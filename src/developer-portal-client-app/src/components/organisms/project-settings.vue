<template lang="pug">
  .card.project-settings
    h2 Project info
    form(@submit.prevent="handleSubmit")
      .flex-wrapper.mb-2
        label Name
          input(type="text" v-model="name" placeholder="Project name" :disabled="status === 2")

        label Web page
          input(type="text" v-model="webpage" placeholder="www.domain.com" :disabled="status === 2")
      .flex-wrapper
        div
          span Upload an image file
          .flex-wrapper
            input.inputfile( type="file" name="file" id="file" @change="previewImage" accept="image/*")
            label.btn.btn-project.btn-small.btn-outline( id="file-label" for="file") Choose a file...
            .logo-preview
              img(:src="imageData || logoUrl || 'data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/PjwhRE9DVFlQRSBzdmcgIFBVQkxJQyAnLS8vVzNDLy9EVEQgU1ZHIDEuMS8vRU4nICAnaHR0cDovL3d3dy53My5vcmcvR3JhcGhpY3MvU1ZHLzEuMS9EVEQvc3ZnMTEuZHRkJz48c3ZnIGVuYWJsZS1iYWNrZ3JvdW5kPSJuZXcgMCAwIDMyIDMyIiBoZWlnaHQ9IjMycHgiIGlkPSJMYXllcl8xIiB2ZXJzaW9uPSIxLjEiIHZpZXdCb3g9IjAgMCAzMiAzMiIgd2lkdGg9IjMycHgiIHhtbDpzcGFjZT0icHJlc2VydmUiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiPjxnIGlkPSJjYW1lcmEiPjxwYXRoIGNsaXAtcnVsZT0iZXZlbm9kZCIgZD0iTTE2LDEwLjAwMWMtNC40MTksMC04LDMuNTgxLTgsOGMwLDQuNDE4LDMuNTgxLDgsOCw4ICAgYzQuNDE4LDAsOC0zLjU4Miw4LThDMjQsMTMuNTgzLDIwLjQxOCwxMC4wMDEsMTYsMTAuMDAxeiBNMjAuNTU1LDIxLjkwNmMtMi4xNTYsMi41MTYtNS45NDMsMi44MDctOC40NTksMC42NSAgIGMtMi41MTctMi4xNTYtMi44MDctNS45NDQtMC42NS04LjQ1OWMyLjE1NS0yLjUxNyw1Ljk0My0yLjgwNyw4LjQ1OS0wLjY1QzIyLjQyLDE1LjYwMiwyMi43MTEsMTkuMzkxLDIwLjU1NSwyMS45MDZ6IiBmaWxsPSIjMzMzMzMzIiBmaWxsLXJ1bGU9ImV2ZW5vZGQiLz48cGF0aCBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGQ9Ik0xNiwxNC4wMDFjLTIuMjA5LDAtMy45OTksMS43OTEtNCwzLjk5OXYwLjAwMiAgIGMwLDAuMjc1LDAuMjI0LDAuNSwwLjUsMC41czAuNS0wLjIyNSwwLjUtMC41VjE4YzAuMDAxLTEuNjU2LDEuMzQzLTIuOTk5LDMtMi45OTljMC4yNzYsMCwwLjUtMC4yMjQsMC41LTAuNSAgIFMxNi4yNzYsMTQuMDAxLDE2LDE0LjAwMXoiIGZpbGw9IiMzMzMzMzMiIGZpbGwtcnVsZT0iZXZlbm9kZCIvPjxwYXRoIGNsaXAtcnVsZT0iZXZlbm9kZCIgZD0iTTI5LjQ5Miw5LjA0MmwtNC4zMzQtMC43MjNsLTEuMzczLTMuNDM0ICAgQzIzLjMyNiwzLjc0LDIyLjIzMiwzLDIxLDNIMTFDOS43NjgsMyw4LjY3NCwzLjc0LDguMjE0LDQuODg2TDYuODQyLDguMzE5TDIuNTA5LDkuMDQyQzEuMDU1LDkuMjgzLDAsMTAuNTI3LDAsMTJ2MTUgICBjMCwxLjY1NCwxLjM0NiwzLDMsM2gyNmMxLjY1NCwwLDMtMS4zNDYsMy0zVjEyQzMyLDEwLjUyNywzMC45NDUsOS4yODMsMjkuNDkyLDkuMDQyeiBNMzAsMjdjMCwwLjU1My0wLjQ0NywxLTEsMUgzICAgYy0wLjU1MywwLTEtMC40NDctMS0xVjEyYzAtMC40ODksMC4zNTQtMC45MDYsMC44MzYtMC45ODZsNS40NDQtMC45MDdsMS43OTEtNC40NzhDMTAuMjI0LDUuMjUsMTAuNTkxLDUsMTEsNWgxMCAgIGMwLjQwOCwwLDAuNzc1LDAuMjQ5LDAuOTI4LDAuNjI5bDEuNzkxLDQuNDc4bDUuNDQ1LDAuOTA3QzI5LjY0NiwxMS4wOTQsMzAsMTEuNTExLDMwLDEyVjI3eiIgZmlsbD0iIzMzMzMzMyIgZmlsbC1ydWxlPSJldmVub2RkIi8+PC9nPjwvc3ZnPg=='")

        label Description
          textarea(placeholder="Description" v-model="description" :disabled="status === 2") {{description}}
      button.btn.btn-right.btn-project.mt-3( :disabled="status === 2") Save

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'

@Component({
  computed: {
    ...mapState('projects', ['status']),
    ...mapGetters('projects', ['currentProject']),
    ...mapActions('projects', ['initCurrentProject'])
  },
  data() {
    return {
      ready: false,
      imageData: null,
      name: '',
      webpage: '',
      description: ''
    }
  },
  async created() {
    await this.$store.dispatch('projects/initCurrentProject')
    this.ready = true
  },
  components: {}
})
export default class ProjectSettingsPage extends Vue {
  private submitted: boolean = false
  private ready: boolean = false
  private name: string
  private webpage: string
  private description: string
  private logoUrl: string = ''
  private imageData: string | ArrayBuffer

  private mounted() {
    this.name = this.$store.state.projects.current.project.name
    this.webpage = this.$store.state.projects.current.project.webpage
    this.description = this.$store.state.projects.current.project.description
    this.logoUrl = this.$store.state.projects.current.project.logoUrl
  }

  private previewImage(event) {
    var input = event.target
    if (input.files && input.files[0]) {
      var reader = new FileReader()
      reader.onload = (e) => {
        this.imageData = reader.result //e.target.result;
        this.logoUrl = this.imageData as string
      }
      reader.readAsDataURL(input.files[0])
    }
  }

  private handleSubmit(e) {
    this.submitted = true
    const self = this
    this.$store.dispatch('projects/updateProject', {
      name: self.name,
      webpage: self.webpage,
      description: self.description,
      logoUrl: self.logoUrl
    })
    .then((result) => {self.submitted = false})
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
 .project-settings {

  input{
    text-overflow:ellipsis;
    @include tiny-screen{
      font-size:1rem;
      padding-right: 0;
    }
  }
  
  .flex-wrapper{
    align-items:center;
    & > * {
      flex-basis:50%;

      .logo-preview{
        flex:1 0 auto;
        align-items:flex-end;
        text-align:center;
        img{
          width:50px;
          height:50px;
        }
      }
      #file-label{
        flex:0 0 auto;
      }

      @include small-screen {
        flex-basis:100%;
      }

      &:not(:last-child){
        margin-right:2rem;
      }
    }
  }
}
</style>
