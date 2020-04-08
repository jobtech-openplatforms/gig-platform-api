<template lang="pug">

      form.box.project.frame(@submit.prevent="handleSubmit")
        .project-settings.project-header(v-bind:class="{ flex : formDisabled}" )
          .has-edit.form-group.project-name
            label.small(for="project-name") Project name
            input.bold.editable(type="text" :value="editingProject.name" id="project-name" @input="updateName" :disabled="formDisabled || testMode" placeholder="Dont forget to give your service a name!")
            span(v-if="!formDisabled && !testMode").
              This is the name of your platform that will be displayed in platform listings, 
              for example: 'Uber Eats'. 
            em(v-if="!formDisabled && testMode") NOTE! Project name can only be edited in LIVE mode
            hr.my-2(v-if="!formDisabled")
          .has-edit.editable.edit-logo
            label#file-label(for="file")
              div.project-logo(v-if="editingProject.logoUrl != null" :style="{'background-image': 'url(' + editingProject.logoUrl + ')'}")
              div.project-logo(v-else)
              input.inputfile(v-if="!formDisabled" :disabled="formDisabled" type="file" name="file" id="file" @change="upload" accept="image/*")
              span(v-if="!formDisabled") This logo will be displayed in platform listings etc. Please use a logotype/icon that looks nice on a square with white background (for example the same icon you'd use on Facebook).
        .project-settings
          hr.my-2
          .has-edit.form-group
            label.small(for="project-webpage") Webpage URL
            input.editable(type="url" id="project-webpage" :value="editingProject.webpage" @input="updateWebpage" :disabled="formDisabled" placeholder="Tell us where to find your service!")
            p(v-if="!formDisabled") The url to the web page of your platform.
          .has-edit.form-group
            label.small(for="project-description") Description
            textarea.editable(placeholder="Give us some quick info about your service!" id="project-description" :value="editingProject.description" @input="updateDescription" :disabled="formDisabled") {{editing.description}}
            p(v-if="!formDisabled") Enter a short description of your platform (max 100 characters).
          .buttons
            button.btn.right.btn-project(v-if="!formDisabled" :disabled="status === 2" type="submit") Save
            button.btn.right.btn-secondary(v-if="!formDisabled" :disabled="status === 2" @click="cancelEdit()") Cancel
            button.btn.right.btn-outline.btn-project(v-if="formDisabled" type="button" key="123456789" @click="enableForm()") Edit...
          slot
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
import { CurrentPlatformState } from '../../store/projects.module'
import ImageTools from '../utils/image-tool'
import { mediaService } from '../../_services'

@Component({
  computed: {
    ...mapState('projects', ['current', 'status', 'editing', 'testMode']),
    ...mapGetters('projects', ['currentProject', 'editingProject'])
  },
  data() {
    return {
      ready: false,
      formDisabled: true,
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
  private formDisabled: boolean = true
  private name: string
  private webpage: string
  private description: string
  private logoUrl: string = ''
  private imageData: string | ArrayBuffer
  private editing: CurrentPlatformState
  private file: string
  private imageSize: number = 1200
  private imageName: string

  public uploadStatus: '' | 'Resizing' | 'Uploading' | 'Completed' = '';
  public uploadPercent;
  public downloadURL;

  $refs: {
    file: HTMLFormElement
  }

  private cancelEdit() {
    this.$store.commit('projects/cancelEdit')
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

        let formData = new FormData()
        formData.append('file', blob, Date.now().toString())

        mediaService.saveFile(formData, this.$store.state.projects.current.project.id)
                    .then(data => {
                      self.uploadStatus = 'Completed'
                      self.$store.commit('projects/localEdit', { value: data.data as string, name: 'logoUrl'})
                    })
      }
    });
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
    const self = this
    this.$store.dispatch('projects/updateProject', self.$store.state.projects.editing.project)
    .then((result) => {
      self.submitted = false
      self.disableForm()
      self.$store.dispatch('projects/getAll')
    })
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
  padding: 2rem;
  max-width: 560px;
  // margin-left: auto;
  // margin-right: auto;
  flex-grow: 1;

  .edit-logo:after {
    color: #fff;
    bottom: 0;
    top: unset;
  }

  .project-logo {
    width: 78px;
    height: 78px;
    flex: 0 0 78px;
    display: block;
    cursor:pointer;
    background-color:#fff;
    margin-right:2rem;
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

.frame {
  padding: $space-med;
  @include flex(column, null, null);

  p {
    flex: 1;
    margin: 0;
  }
}

.delimiter {
  margin: 4rem 0;
}

h4 {
  margin-left: 2rem;
}

.project-settings.project-header.flex{
  @include flex(row-reverse, flex-start, center);
  .edit-logo{
    @include flex-child(0, 1, auto);
  }
  .project-name{
    @include flex-child(1, 0, auto);
    input{
      font-size:$font-big;
      font-family: $serif;
    }
  }
}
  #file-label{
    @include flex(row, flex-start, center);
  }
</style>
