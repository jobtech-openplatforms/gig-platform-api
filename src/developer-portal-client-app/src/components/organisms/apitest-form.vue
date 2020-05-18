<template lang="pug">
  .api-test
    div(v-if="testStatus < 2")
      h2 Test your API
      p.flex-wrapper.test-instructions
        span In order to test if your server works correctly, you need supply an email address for a test user on your platform with at least one gig.
        button.btn.btn-right.btn-outline.btn-export.btn-small.my-2.ml-4(@click="toggleInstructions()") {{!showInstructions ? 'Show instructions &#9660' : 'Hide instructions &#9650'}}
      hr.spacious
      PlatformInstructions(v-if="showInstructions")

    div(v-if="testStatus == 4")
      .success-header
        h1 Success!
          span.ml-1(v-if="currentPlatform.published" class="color-export").
            This platform is live on Open Platforms
      .flex-wrapper.success-info(v-if="!currentPlatform.published")
        p.
          It looks like your implementation has succeeded! Take a look at
          the user data below. Does everything look alright?
          #[span(v-if="!testMode") If so, you are ready to go live with your data openness!]
        .platform-pending.mb-2(v-if="!testMode")
          GoLiveButton(v-if="currentProjectCompleted")
          button.btn.btn-huge.btn-export.btn-continue.center.ml-4(v-else @click="openModal('project-details')") Continue
      p.mt-2(v-if="currentProjectCompleted && !currentPlatform.published && !testMode").
          #[strong PLEASE NOTE]: After you press the 'Go Live' button,
          your service will be added to Open Platform's list of connected
          platforms, and your users will be able to make data requests.
      hr.mb-2

      h2 Test result
      p Result with user&nbsp;
        em {{test.email}}

    div(v-if="testStatus == 3")
      h2 Oh no!
      p Something went wrong. Here is some more information on what happened:
      .frame.my-4
        pre {{ testError }}

      p Make some changes accordingly, and run the test again!

    .card.my-4(v-if="currentPlatform && currentPlatform.exportDataUri && (testStatus === 1 || testStatus === 3)")
      form.inline( @submit.prevent="handleSubmit")
        .form-group
          label(for="api-test-email")
            .label-text Test user email
            input(type="email" id="api-test-email" :disabled="testStatus === 2" v-model="test.email" name="testemail" placeholder="existinguser@yourplatform.com")
          .feedback
            .invalid-feedback(v-if="error") {{ error }}
        button.btn.btn-export.btn-right(v-if="!submitted" key="78910" :disabled="testStatus === 2") Run the test!
    h2(v-if="(submitted && !completed) || testStatus === 2") Performing test...

    .card(v-if="currentPlatform && !currentPlatform.exportDataUri")
      h2 Missing URL
      p You have to enter a valid URL for the #[strong Export data URL] in the settings
      router-link.btn.btn-primary(to="/project") Back to Settings


    form.inline.card.mb-4(v-bind:class="{ 'form-inactive': editUrlDisabled && currentPlatform }" @submit.prevent="saveUrl")
      .form-group
        label.label-muted(for="exportDataUri" @click="enableForm()")  Project export data url
        input(v-if="currentPlatform" type="url" name="exportDataUri" :value="currentPlatform.exportDataUri" @input="newUrl = $event.target.value" :disabled="editUrlDisabled" placeholder="Project export data url")
      button.btn.right.btn-export(v-if="!editUrlDisabled"  key="7890") Save
      button.btn.right.btn-outline.btn-export.btn-small(v-if="editUrlDisabled"  key="7891" type="button" @click="enableForm()") Edit...

    .token-keys(v-if="currentProject.platforms && (testStatus === 1 || testStatus === 3)")
      PlatformToken

    .flex-wrapper#result(v-if="testStatus > 1")

      .card(v-if="testStatus <= 1" key='1')
        hr
        //- h2 Test results
        //- p Awaiting test ...

      .tech-box(v-else-if="testStatus == 4" key='4')
        h3 Request results
        .card
          div(v-if="testResult && testResult.response")
            div(v-if="testResult.request" )
              strong Request
              p.small(v-for="s in testResult.request.headers") {{ s }}
            div(v-if="testResult.response")
              strong Response
              div
                u Headers
                p.small Status: {{ testResult.response.status }}
                p.small(v-for="s in testResult.response.headers") {{ s }}

      div.visual-result(v-if="testResult.result" key='5')
        h3 Reputation
        .card.reputation(v-for="i in visualInteractions()")
          span.project-logo.logo(v-if="currentProject.logoUrl != null && currentProject.logoUrl != ''" :style="{'background-image': 'url(' + currentProject.logoUrl + ')'}")
          span.project-logo(v-else)
          .result-info
            strong {{ currentProject.name || "Platform name" }}
            p.stars
              template(v-for="index in i.maxRatingValue")
                span(v-if="index > i.averageRating") ☆
                span(v-if="index <= i.averageRating") ★
            em.small {{i.averageRating}} från {{i.client.name}}
          //- p {{i}}
        h3.mt-4 Certificates
        .card.achievement(v-for="i in testResult.result.achievements")
          span.project-logo.badge(v-if="i.badgeIconUri != null && i.badgeIconUri != ''" :style="{'background-image': 'url(' + i.badgeIconUri + ')'}")
          span.project-logo.logo(v-else-if="currentProject.logoUrl != null && currentProject.logoUrl != ''" :style="{'background-image': 'url(' + currentProject.logoUrl + ')'}")
          span.project-logo(v-else)
          .result-info
            strong {{ currentProject.name || "Platform name" }}
            h4 {{i.name}}
            em.small {{i.description}}
            //- p {{i}}

    div.mt-4(v-if="testResult.result")
      h3 JSON result
      .card
        pre(v-if="testResult.result.interactions") {{ JSON.stringify(testResult.result, null, 2) }}

    modal#project-details(name="project-details" height="auto" :scrollable="true")
      div.mt-2.mx-4
        h2 Complete project info to continue
        p.
          To continue and publish the platform on Open Platforms,
          please fill out the project details.
      ProjectDetails(:formDisabled="false")
</template>

<script lang="ts">
import { mapState, mapActions, mapGetters, mapMutations } from 'vuex'
import GoLiveButton from '../molecules/go-live-button.vue'
import PlatformToken from '../molecules/platform-token.vue'
import PlatformInstructions from '../molecules/platform-instructions.vue'
import ProjectDetails from '../organisms/project-edit.vue'

export default {
  computed: {
    ...mapState('projects', ['current', 'status', 'testStatus', 'testMode']),
    ...mapGetters('projects', [
      'currentProject',
      'currentPlatform',
      'testResult',
      'testError',
      'currentProjectCompleted'
    ]),
    ...mapMutations('project', ['resetTest']),
    formDisabled() {
      return this.editUrlDisabled || this.testStatus === 2 || this.submitted
    }
  },
  mounted() {
    this.$store.commit('projects/resetTest')
    // if (!this.currentPlatform || !this.currentPlatform.exportDataUri) {
    //     this.$router.push('/platform-settings')
    // }
  },
  components: {
    GoLiveButton,
    PlatformToken,
    PlatformInstructions,
    ProjectDetails
  },
  data() {
    return {
      test: {
        name: '',
        email: ''
      },
      submitted: false,
      completed: false,
      editUrlDisabled: true,
      showInstructions: false,
      newUrl: '',
      error: '',
      result: {}
    }
  },
  methods: {
    ...mapActions('projects', ['testPlatform', 'setPlatformUrl']),
    toggleInstructions() {
      this.showInstructions = !this.showInstructions
    },
    visualInteractions() {
      if (
        this.testResult == null ||
        this.testResult.result == null ||
        this.testResult.result.interactions == null
      ) {
        return []
      }
      // tslint:disable-next-line:only-arrow-functions
      return this.testResult.result.interactions.map(function(i) {
        return {
          client: i.client,
          // ratings: i.outcome.ratings,
          averageRating:
            i.outcome.ratings.reduce((sum, { value }) => sum + value, 0) /
            i.outcome.ratings.length,
          maxRatingValue: Math.max.apply(
            Math,
            i.outcome.ratings.map((o) => o.max)
          )
        }
      })
    },
    handleSubmit(e) {
      if (this.submitted) {
        return false
      }
      if (this.test.email === '') {
        this.error =
          'Please enter a valid email address of a user that exists on your platform'
        return false
      }
      this.submitted = true
      this.error = ''
      // 1. Change display state to fetching
      this.showform = false
      // 2. Prevent simultaneous calls
      // 3. Call API
      this.testPlatform({
        id: this.currentPlatform.id,
        username: this.test.email
      })
        .then((response) => {
          this.showform = true
          this.completed = true
          this.submitted = false
        })
        .catch((error) => {
          alert(error)
          this.completed = true
          this.submitted = false
        })
      // 4. On response or error, change display state
      // 5. Show response
    },
    saveUrl(e) {
      if (this.submitted) {
        return false
      }
      this.submitted = true
      this.editUrlDisabled = true
      this.error = ''
      this.setPlatformUrl(this.newUrl)
        .then((response) => {
          this.submitted = false
        })
        .catch((error) => {
          alert(error)
          this.submitted = false
        })
    },
    newTest() {
      this.submitted = this.completed = false
      this.error = ''
    },
    cancelTest() {
      this.submitted = this.completed = false
      this.error = ''
    },
    cancelEdit() {
      // this.$store.commit('projects/cancelEdit')
      this.disableForm()
    },
    disableForm() {
      this.editUrlDisabled = true
    },
    enableForm() {
        this.editUrlDisabled = false
    },
    openModal(modal) {
      this.$modal.show(modal)
    }
  }
}
</script>


<style lang="scss">
.success-header {
  @include flex(row, space-between, flex-end);
  h2 {
    font-size: $font-huge;
    margin-bottom: 0;
  }
}

.success-info p{
  flex:1 1 400px;
}

.tech-box {
  flex: 1 1 300px;
  order: 2;
  margin-left: 4rem;
}

.visual-result {
  flex:0 0 300px;
  order: 1;
  .card {
    padding: 1rem;
    @include flex(row, null, null);
    .result-info {
      margin-left: 1rem;
      @include flex(column, space-between, null);
      p,
      h4 {
        margin: 0;
      }

      .stars {
        color: $orange;
        font-size: 3rem;
      }
    }
    .project-logo{
      width: 80px;
      height: 80px;
      border: 1px solid $border-grey;
      flex:0 0 auto;
    }
  }
}

#project-details{
  .project{
    @include box-spacing(margin);
    background:$light-grey;
  }
}



</style>
