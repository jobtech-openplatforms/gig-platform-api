<template lang="pug">
  div.home(v-if="ready")
    div(v-if="currentApplication")
      div(v-if="currentApplication.authCallbackUrl")
          h1 Application settings
          p.flex-wrapper.test-instructions
            span.
              To help you test the application you are developing to retrieve data from
              platforms for a user, we have created some test methods you can use to
              send test data to receive in your application.
            button.btn.btn-right.btn-outline.btn-import.btn-small.m-2.mr-0(@click="toggleInstructions()") {{!showInstructions ? 'Show instructions &#9660' : 'Hide instructions &#9650'}}
          AppInstructions(v-if="showInstructions")

          p.
            #[strong Tip!] #[em To test while developing on localhost, consider using a service like] #[a(href="https://ngrok.com/" target="_blank") ngrok] #[em to receive data from Open Platforms to your development machine.]
          hr.my-2

      div(v-else)
        h1 Application API
        p.
          This API can be used both to let your user's to share 
          their data with your application, and to let your user's 
          connect additional platforms to their Open Platforms account. 
        p.
          Read the #[router-link.color-import(to="/application-documentation") documentation for developers].
        p.
          Read more about the API in the
          #[a.color-import(href="https://gigdata-api.openplatforms.org" target="_blank")  Application API Documentation]


      p(v-if="!currentApplication.authCallbackUrl") To use the API you'll need these API keys:
      current-application-tokens

      p.my-4(v-if="!currentApplication.authCallbackUrl") #[strong PLEASE NOTE] The Application-API is still in beta, the basic concept will stay the same but there might be minor changes to the API endpoints.

      h3 Application endpoints
      div
        form.card(v-bind:class="{ 'form-inactive': formDisabled }" @submit.prevent="handleSubmit" v-if="status !== 2")
          .form-group
            label(for="auth-callback-url")
              .label-text(v-bind:class="{ 'label-muted': formDisabled }") Auth callback URL
                //- span(v-if="current.loading")  (Loading...)
              input(type="url"
                :placeholder="currentApplication ? currentApplication.authCallbackUrl : 'https://yourdomain.com/auth-callback-url'"
                :disabled="formDisabled"
                :class="{ error: errorsContains('auth-callback-url') }"
                required
                id="auth-callback-url"
                v-model="authCallbackUrl")
            .help-text(v-if="!formDisabled") As part of the login flow the user is redirected to an url that you specify (to make sure the user is not tricked to login to an other app)
            .feedback
          .form-group
            label(for="data-update-callback-url")
              .label-text(v-bind:class="{ 'label-muted': formDisabled }") Data update callback URL
              input(type="url"
                :placeholder="currentApplication ? currentApplication.dataUpdateCallbackUrl : 'https://yourdomain.com/data-update-callback-url'"
                :disabled="formDisabled"
                :class="{ error: errorsContains('data-update-callback-url') }"
                required
                id="data-update-callback-url"
                v-model="dataUpdateCallbackUrl")
            .help-text(v-if="!formDisabled") After connecting to the API we will send automatic updates when an user's data has changed. Enter the URL that you want us to use for updates.
            .feedback

          .form-unsaved(v-if="formEdited")
            p.error Recent edits to the urls have not been saved
          .buttons.mb-2
            button.btn.btn-import.right(v-if="!formDisabled" type="submit") Save
            button.btn.btn-secondary.right(v-if="!formDisabled && (authCallbackUrl||dataUpdateCallbackUrl)" @click="cancelEdit()" type="reset") Cancel
            button.btn.btn-import.btn-outline.right.small(v-if="formDisabled" key="123" type="button" @click="enableForm()") Edit...

          .error(v-if="!formDisabled") {{current.error && current.error.message ? current.error.message : current.error}}
        div(v-else)
          h2 Saving...
          p
            strong Auth callback URL
            pre.imported {{authCallbackUrl}}
          p
            strong Data update callback URL
            pre.imported {{dataUpdateCallbackUrl}}

    modal#project-details(name="project-details" height="auto" maxWidth="600px" width="95%" :scrollable="true")
      .m-2
        h2 Complete project info to continue
        p.
          To continue and publish the application,
          please fill out the project details.
      ProjectDetails
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
import { ApplicationState } from '../store/projects.module'
import CurrentApplicationTokens from '../components/organisms/current-application-tokens.vue'
import AppInstructions from '../components/organisms/app-instructions.vue'
import ProjectDetails from '../components/organisms/project-edit.vue'

@Component({
  computed: {
    ...mapState('projects', ['status', 'current']),
    ...mapGetters('projects', ['currentApplication', 'currentProjectCompleted'])
  },
  data() {
    return {
      ready: false,
      submitted: false,
      formDisabled: true,
      showInstructions: false
    }
  },
  beforeUpdate() {
    if (!this.currentApplication) {
      this.$store.dispatch('projects/createApplication')
    }
  },
  created() {
    if (!this.currentApplication) {
      this.$store.dispatch('projects/createApplication')
    }
    this.$store.dispatch('projects/initCurrentProject')
    this.ready = true
  },
  components: {
    CurrentApplicationTokens,
    ProjectDetails,
    AppInstructions
  }
})
export default class IntegrateUserDataPage extends Vue {
  private submitted: boolean = false
  private ready: boolean = false
  private authCallbackUrl: string = ''
  private dataUpdateCallbackUrl: string = ''
  private currentApplication: any
  private formDisabled: boolean = true
  private formEdited: boolean = false
  private showInstructions: boolean = false
  // private currentProjectCompleted: boolean = false

  private async mounted() {
    await this.$store.dispatch('projects/initCurrentProject')
    if (!this.currentApplication) {
      this.$store.commit('projects/queueDispatchAfterInit', 'createApplication')
    }
    this.authCallbackUrl = this.currentApplication
      ? this.currentApplication.authCallbackUrl
      : ''
    this.dataUpdateCallbackUrl = this.currentApplication
      ? this.currentApplication.dataUpdateCallbackUrl
      : ''
    this.formDisabled = this.currentApplication !== null && this.currentApplication.authCallbackUrl != null
    if(!this.currentApplication)
      this.$store.commit('projects/queueDispatchAfterInit', 'createApplication')
  }

  private isFormEdited() {
    if (!this.currentApplication) {
      return (
        this.authCallbackUrl !== '' ||
        this.dataUpdateCallbackUrl !== ''
      )
    }
    return (
      this.currentApplication.authCallbackUrl !== this.authCallbackUrl ||
      this.currentApplication.dataUpdateCallbackUrl !==
        this.dataUpdateCallbackUrl
    )
  }

  private errorsContains(n) {
    return (
      this.$store.state.projects.current.error &&
      this.$store.state.projects.current.error.errors &&
      this.$store.state.projects.current.error.errors.indexOf(n) > -1
    )
  }

  private handleSubmit(e) {
    this.submitted = true

    if (!this.currentProjectCompleted) {
      this.$modal.show('project-details')
    }
    this.$store
      .dispatch('projects/setApplicationUrls', {
        authCallbackUrl: this.authCallbackUrl,
        dataUpdateCallbackUrl: this.dataUpdateCallbackUrl
      })
      .then((result) => {
        this.submitted = false
      })
      .catch((error) => {
        this.submitted = false
        this.$store.dispatch('alert/error', error, { root: true })
      })
  }
  private cancelEdit() {
    if (this.currentApplication) {
      this.disableForm()
      this.$store.commit('projects/cancelEdit')
      this.authCallbackUrl = this.currentApplication.authCallbackUrl
      this.dataUpdateCallbackUrl = this.currentApplication.dataUpdateCallbackUrl
    } else {
      this.authCallbackUrl = ''
      this.dataUpdateCallbackUrl = ''
    }
  }
  private disableForm() {
    this.formDisabled = true
    this.formEdited = this.isFormEdited()
  }
  private enableForm() {
    this.formEdited = false
    if (this.formDisabled) {
      this.formDisabled = false
    }
    return false
  }

  toggleInstructions() {
    this.showInstructions = !this.showInstructions
  }

  openModal(modal) {
    this.$modal.show(modal)
  }
}
</script>

<style lang="scss" scoped>
input {
  &.error {
    border: 1px solid red;
  }
}
.error {
  color: red;
}
</style>
