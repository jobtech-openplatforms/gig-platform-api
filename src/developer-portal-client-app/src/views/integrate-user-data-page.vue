<template lang="pug">
  div.home(v-if="ready")
    div(v-if="currentApplication")
      div(v-if="currentApplication.authCallbackUrl")
          h1 Test your application
          p.flex-wrapper
            span.
              To help you test the application you are developing to retrieve data from
              platforms for a user, we have created some test methods you can use to
              send test data to receive in your application.
            button.btn.btn-right.btn-outline.btn-import.btn-small.mb-2.ml-4(@click="toggleInstructions()") {{!showInstructions ? 'Show instructions &#9660' : 'Hide instructions &#9650'}}
          AppInstructions(v-if="showInstructions")

          p.
            #[strong Tip!] #[em To test while developing on localhost, consider using a service like] #[a(href="https://ngrok.com/" target="_blank") ngrok] #[em to receive data from Open Platforms to your development machine.]
          hr.my-2

      div(v-else)
        h1 Application API
        p This API can be used both to let your user's to share their data with your application, and to let your user's connect additional platforms to their Open Platforms account. You can read more about the API in the
          strong
            a.color-import(href="https://gigdata-api.openplatforms.org" target="_blank")  Application API Documentation
      
      
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
            label(for="gig-data-notification-url")
              .label-text(v-bind:class="{ 'label-muted': formDisabled }") Gig data notification URL
              input(type="url"
                :placeholder="currentApplication ? currentApplication.gigDataNotificationUrl : 'https://yourdomain.com/gig-data-notification-url'"
                :disabled="formDisabled"
                :class="{ error: errorsContains('gig-data-notification-url') }"
                required
                id="gig-data-notification-url"
                v-model="gigDataNotificationUrl")
            .help-text(v-if="!formDisabled") After connecting to the API we will send automatic updates when an user's data has changed. Enter the URL that you want us to use for updates.
            .feedback
          .form-group
            label(for="email-verification-url")
              .label-text(v-bind:class="{ 'label-muted': formDisabled }") E-mail verification URL
              input(type="url"
                :placeholder="currentApplication ? currentApplication.emailVerificationUrl : 'https://yourdomain.com/email-verification-url'"
                :disabled="formDisabled"
                :class="{ error: errorsContains('email-verification-url') }"
                required
                id="email-verification-url"
                v-model="emailVerificationUrl")
              .help-text(v-if="!formDisabled") If you let your user's connect new platforms through the API, Open Platform will verify the user's email address. If you want to recieve notifications when the user has confirmed their address, enter a url in this field.
              .feedback
          .form-unsaved(v-if="formEdited")
            p.error Recent edits to the urls have not been saved 
          .buttons.mb-2
            button.btn.btn-import.right(v-if="!formDisabled" type="submit") Save
            button.btn.btn-secondary.right(v-if="!formDisabled && (authCallbackUrl||gigDataNotificationUrl||emailVerificationUrl)" @click="cancelEdit()" type="reset") Cancel
            button.btn.btn-import.btn-outline.right.small(v-if="formDisabled" key="123" type="button" @click="enableForm()") Edit...

          .error(v-if="!formDisabled") {{current.error && current.error.message ? current.error.message : current.error}}
        div(v-else)
          h2 Saving...
          p
            strong Auth callback URL
            pre.imported {{authCallbackUrl}}
          p
            strong Gig data notification URL
            pre.imported {{gigDataNotificationUrl}}
          p
            strong E-mail verification URL
            pre.imported {{emailVerificationUrl}}

    modal#project-details(name="project-details" height="auto" :scrollable="true") 
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
  methods: {
    toggleInstructions() {
      this.showInstructions = !this.showInstructions
    }
  },

  created() {
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
  private gigDataNotificationUrl: string = ''
  private emailVerificationUrl: string = ''
  private currentApplication: any
  private formDisabled: boolean = true
  private formEdited: boolean = false

  private async mounted() {
    await this.$store.dispatch('projects/initCurrentProject')
    this.authCallbackUrl = this.currentApplication
      ? this.currentApplication.authCallbackUrl
      : ''
    this.gigDataNotificationUrl = this.currentApplication
      ? this.currentApplication.gigDataNotificationUrl
      : ''
    this.emailVerificationUrl = this.currentApplication
      ? this.currentApplication.emailVerificationUrl
      : ''
    this.formDisabled = this.currentApplication !== null && this.currentApplication.authCallbackUrl != null
    if(!this.currentApplication)
      this.$store.commit('projects/queueDispatchAfterInit', 'createApplication')
    console.log("formDisabled: " + this.formDisabled);
  }

  private isFormEdited() {
    if (!this.currentApplication) {
      return (
        this.authCallbackUrl !== '' ||
        this.gigDataNotificationUrl !== '' ||
        this.emailVerificationUrl !== ''
      )
    }
    return (
      this.currentApplication.authCallbackUrl !== this.authCallbackUrl ||
      this.currentApplication.gigDataNotificationUrl !==
        this.gigDataNotificationUrl ||
      this.currentApplication.emailVerificationUrl !== this.emailVerificationUrl
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
      this.$modal.show("project-details")
    }
    this.$store
      .dispatch('projects/setApplicationUrls', {
        authCallbackUrl: this.authCallbackUrl,
        gigDataNotificationUrl: this.gigDataNotificationUrl,
        emailVerificationUrl: this.emailVerificationUrl
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
      this.gigDataNotificationUrl = this.currentApplication.gigDataNotificationUrl
      this.emailVerificationUrl = this.currentApplication.emailVerificationUrl
    } else {
      this.authCallbackUrl = ''
      this.gigDataNotificationUrl = ''
      this.emailVerificationUrl = ''
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
