<template lang="pug">

      form.project.frame(@submit.prevent="handleSubmit")
        .project-settings.project-header(v-bind:class="{ flex : formDisabled}" )
          .form-group.project-name
            label.small(for="project-name") Project name
            input.bold(type="text" :value="editingProject.name" id="project-name" @input="updateName" :disabled="formDisabled || devMode" placeholder="Dont forget to give your service a name!" maxlength="20")
            span(v-if="!formDisabled && !devMode").
              This is the name of your platform that will be displayed in platform listings,
              for example: 'Uber Eats'.
            em(v-if="!formDisabled && devMode") NOTE! Project name can only be edited in LIVE mode
            hr.my-2(v-if="!formDisabled")
          .edit-logo
            div.mr-4
              div.project-logo(v-if="editingProject.logoUrl != null" :style="{'background-image': 'url(' + editingProject.logoUrl + ')'}")
              div.project-logo(v-else)
              label#file-label.btn-tiny.btn-outline.btn-project(for="file" v-if="!formDisabled" v-bind:class="{ editable : !formDisabled}") Upload
            input.inputfile(v-if="!formDisabled" :disabled="formDisabled" type="file" name="file" id="file" @change="upload" accept="image/*")
            label#file-label(v-if="!formDisabled") This logo will be displayed in platform listings etc. Please use a logotype/icon that looks nice on a square with white background (for example the same icon you'd use on Facebook).
        .project-settings
          hr.my-2
          .form-group
            label.small(for="project-webpage") Webpage URL
            input(type="url" id="project-webpage" :value="editingProject.webpage" @input="updateWebpage" :disabled="formDisabled" placeholder="https://your-domain.tld/...")
            p(v-if="!formDisabled") The url to the web page of your platform.
          .form-group
            label.small(for="project-description") Description
            textarea(placeholder="Give us some quick info about your service!" id="project-description" rows="3" :value="editingProject.description" @input="updateDescription" :disabled="formDisabled" maxlength="100") {{editing.description}}
            p(v-if="!formDisabled") Enter a short description of your platform (max 100 characters).
          .buttons.mb-2
            button.btn.right.btn-project(v-if="!formDisabled" :disabled="status === 2" type="submit") Save
            button.btn.right.btn-secondary(v-if="!formDisabled" :disabled="status === 2" @click="cancelEdit()") Cancel
            button.btn.right.btn-outline.btn-project(v-if="formDisabled" type="button" key="123456789" @click="enableForm()") Edit...
          slot
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
import { CurrentPlatformState } from '../../store/projects.module'
import ImageTools from '../utils/image-tool'
import { mediaService } from '../../_services'

@Component({
  computed: {
    ...mapState('projects', ['current', 'status', 'editing', 'devMode']),
    ...mapGetters('projects', ['currentProject', 'editingProject'])
  },
  data() {
    return {
      ready: false,
      // formDisabled: true,
      imageData: null,
      name: '',
      webpage: '',
      description: '',
      logoUrl: ''
    }
  },
  // mounted() {
  //   this.$store.dispatch('projects/getAll')
  // },
  components: {}
})
export default class ProjectEdit extends Vue {
  private submitted: boolean = false
  private ready: boolean = true
  @Prop({ default: true }) private formDisabled: boolean
  private name: string
  private webpage: string
  private description: string
  private logoUrl: string = ''
  private imageData: string | ArrayBuffer
  private editing: CurrentPlatformState
  private file: string
  private imageSize: number = 1200
  private imageName: string

  public uploadStatus: '' | 'Resizing' | 'Uploading' | 'Completed' = ''
  public uploadPercent
  public downloadURL

  $refs: {
    file: HTMLFormElement
  }

  private cancelEdit() {
    this.$store.commit('projects/cancelEdit')
    this.$modal.hide('project-details')
    this.disableForm()
  }
  private disableForm() {
    this.formDisabled = true
  }
  private enableForm() {
    if (this.formDisabled) {
      this.formDisabled = false
    }
  }

  private upload(event) {
    this.uploadStatus = 'Resizing'
    const self = this
    ImageTools.resize(event.target.files[0], {
      width: this.imageSize, // maximum width
      height: this.imageSize // maximum height
    }, (blob, didItResize) => {
      if (didItResize) {
        this.uploadStatus = 'Uploading'

        const formData = new FormData()
        formData.append('file', blob, Date.now().toString())

        mediaService.saveFile(formData, this.$store.state.projects.current.project.id)
                    .then(data => {
                      self.uploadStatus = 'Completed'
                      self.$store.commit('projects/localEdit', { value: data.data as string, name: 'logoUrl'})
                    })
      }
    })
  }

  private updateName(e) {
    this.$store.commit('projects/localEdit', {
      value: e.target.value,
      name: 'name'
    })
  }
  private updateWebpage(e) {
    this.$store.commit('projects/localEdit', {
      value: e.target.value,
      name: 'webpage'
    })
  }
  private updateDescription(e) {
    this.$store.commit('projects/localEdit', {
      value: e.target.value,
      name: 'description'
    })
  }

  private handleSubmit(e) {
    this.submitted = true
    this.disableForm()
    const self = this
    this.$store.dispatch('projects/updateProject', self.$store.state.projects.editing.project)
    .then((result) => {
      self.submitted = false
      // self.disableForm()
      self.$store.dispatch('projects/getAll', self.$store.state.projects.editing.project)

    })
    this.$modal.hide('project-details')
  }

  private bufferToBase64(buffer) {
    return btoa(
      new Uint8Array(buffer).reduce((data, byte) => {
        return data + String.fromCharCode(byte)
      }, '')
    )
  }
}
</script>
<style lang="scss">

.project {
  padding-bottom:0;

  .edit-logo{
    @include flex(row, flex-start, flex-start);
    &.editable{
      cursor:pointer;
    }
    &:after {
      color: #fff;
      bottom: 0;
      top: unset;
    }
  }


  .project-settings .project-logo {
    width: 78px;
    height: 78px;
    flex: 0 0 78px;
    display: block;
    background-color:#fff;
  }


  .integration-status {
    @include flex(row, null, center);
    margin: 2rem 0;

    .connection-type {
      flex: 1;
    }

    .btn {
      margin-left: auto;
    }
  }
}

.project-settings.project-header.flex{
  @include flex(row-reverse, flex-start, center);
  .edit-logo{
    @include flex-child(0, 1, auto);
  }
  .project-name{
    @include flex-child(1, 1, auto);
    input{
      font-size:$font-big;
      font-family: $serif;
      text-overflow:ellipsis;
    }
  }
  @include tiny-screen{
    display:block;
    .project-name input{
      font-size:$font-med;
    }
  }
}
  #file-label{
    @include flex(row, center, flex-start);
    margin-top:1rem;
  }
</style>
