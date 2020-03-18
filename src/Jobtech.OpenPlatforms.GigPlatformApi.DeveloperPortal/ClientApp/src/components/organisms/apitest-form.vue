<template lang="pug">
  .api-test
    div(v-if="testStatus < 2")
      h2 Test your API
      p.
        In order to test if your server works correctly, you need supply
        an email address for a test user on your platform with at least one gig.
      hr.my-2
      button.btn.btn-right.btn-outline.btn-export.btn-small.mb-2(@click="toggleInstructions()") {{!showInstructions ? 'Show instructions &#9660' : 'Hide instructions &#9650'}}
      hr.my-2
      PlatformInstructions(v-if="showInstructions")

    div(v-if="testStatus == 4")
      .success-header.mb-2
        h2 Success!
      hr.mb-2

      p.
        It looks like your implementation has succeeded! Take a look at
        the user data below. Does everything look alright? If so, you
        are ready to go live with your data openness!
      hr.mb-2

      .platform-live(v-if="currentPlatform.published")
        h2(class="color-export").
          This platform has is live on Open Platforms
      .platform-pending(v-else)
        p(v-if="currentProjectCompleted").
          #[strong PLEASE NOTE]: After you press the 'Go Live' button,
          your service will be added to Open Platform's list of connected
          platforms, and your user's will be able to make data requests.
        GoLiveButton(v-if="currentProjectCompleted")
        p(v-else class="error").
          #[strong PLEASE NOTE]: To publish your platform in the Open
          Platforms service, please fill out all of the
          #[router-link(to="/project") project details]
          like your platform's webpage, description and a logo, and run
          the test again.

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
        button.btn.btn-export.btn-right(v-if="!submitted" :disabled="testStatus === 2") Run the test!
        button.btn.btn-export.btn-right(v-else @click="newTest()" :disabled="testStatus === 2") New Test
      button.btn.btn-primary.btn-right( :disabled="testStatus === 2" v-if="submitted && !completed" @click="cancelTest()") Cancel
    h2(v-if="(submitted && !completed) || testStatus === 2") Performing test...

    .card(v-if="!currentPlatform.exportDataUri")
      h2 Missing URL
      p You have to enter a valid URL for the #[strong Export data URL] in the settings
      router-link.btn.btn-primary(to="/project") Back to Settings

    
    form.card.mb-4(v-bind:class="{ 'form-inactive': formDisabled && currentPlatform }" @submit.prevent="saveUrl")
      .form-group
        label.label-muted(for="exportDataUri" @click="enableForm()")  Project export data url
        .inline
            .has-edit.form-group
              input.editable(v-if="currentPlatform" type="text" name="exportDataUri" :value="currentPlatform.exportDataUri" @input="newUrl = $event.target.value" :disabled="formDisabled" placeholder="Project export data url")
            button.btn.right.btn-export(v-if="!formDisabled") Save
        button.btn.right.btn-outline.btn-export.btn-small(v-if="formDisabled" type="button" @click="enableForm()") Edit...

    .token-keys(v-if="currentProject.platforms && (testStatus === 1 || testStatus === 3)")
      PlatformToken

    .result-wrapper(v-if="testStatus > 1")

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
          img(:src="currentProject.logoUrl || 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAHgAAAB4CAYAAAA5ZDbSAAAIcklEQVR4nO2d7UtT7x/H33qq0zZbWWZGKuVAUqxJRWV7sHUjGUGSGD0Iip5EBfkHSRLeEJgohEhaQmnEMm9yaNNGBnOrvGlrW9Npzu33IPzhV52enZudc11dr6de5/N5s5fbuc451zknLR6Px8GglnS1AzCUhQmmHCaYcphgymGCKYcJphwmmHKYYMphgimHCaYcJphymGDKYYIphwmmHCaYcphgymGCKYcJphwmmHKYYMphgilnm9oBpBIMBjE7O4twOIzFxUUsLy+LrsVxHHieh8FgQFZWFjIzM2VMqg7ECY7FYnA6nRgZGYHL5cLc3JxivXQ6HUwmE0pKSnDs2DFs375dsV5KkUbKuuilpSXY7Xb09PTg9+/fKe/P8zzKyspgtVphMBhS3l8sRAh2OBxob29HKBRSOwp4nkdFRQUsFovaUQShacELCwtoaWnByMiI2lHWUVhYiFu3bkGn06kdZVM0KzgcDqO2thZTU1NqR0lITk4O7t+/D71er3aUhGjyMOnPnz94/PixpuUCwNTUFJ48eSJp5q40mhT89OlTfP/+Xe0YgnC73Whra1M7RkI0J7i3txdOp1PtGEnR39+PDx8+qB1jQzQl2OfzoaurS+0Yomhvb8evX7/UjrEOTQlubW3F0tKS2jFEsbi4iNbWVrVjrEMzgsfHx/Hlyxe1Y0jC5XJhbGxM7Rj/QTOCOzs71Y4gCx0dHWpH+A+aEDw+Pk7MrHkrZmZm8PnzZ7Vj/B9NCO7v71c7gqxoaUYt69WkWCwGv9+PcDiMWCwmeDst/cfLgcvlwtevXwWPT09PR0ZGBrKysmTPIsupynA4jO7ubgwPD2N+fl6OXP8ker0epaWluHTpEjIyMmSpKVnwxMQEGhoaEIlEZAnE+Hsd+vbt2zCZTJJrSdoHu91u1NXVMbkyE4lEUFdXh8nJScm1RAteWlpCU1MTotGo5BCM9USjUTQ2Nko+8SNacF9fH4LBoKTmjM0JBoPo6+uTVEO0YC1ehKcRqZ+zaMFav1ZLC9PT05K2F30cvLCwIKkxbZSXlyMnJ2fDvwUCAbS3t4uqK3UCK1qwRlf6qIbJZEJBQcGGf5PyLZT6OWviVCVDOZhgymGCKYe4W1eSxWg0Yt++fdDr9di2bRui0Sjm5+fh8/k0sZBeaagTzHEcioqKYDabYTKZNj1pHw6HMTExAYfDgbGxMU0vfxULNYI5jsO5c+dgtVphNBoFbZORkQGz2Qyz2YxQKIQ3b97AbreLEh2JRBJeSVPzXD0VgvPz83Hz5k3s379fdA2j0Yhr167h7NmzaG5uhsfjSWr7+vp60b2VhPhJlsViwYMHDyTJXU12djYePnyIsrIyWeqpDdHf4CtXruD8+fOy1+U4DtevX4fRaCR2nfYKxH6Dy8vLFZG7mosXL+LChQuK9lAaIgWXlpaivLw8Jb0qKipw/PjxlPRSAuIE7927F9XV1SnteePGDWKf10Gc4MrKSuzYsSOlPXmeR2VlZUp7ygVRk6zDhw+jqKgoqW1+/vyJ0dFR/PjxA5FIBDqdDgcPHkRJSUlSy1SLi4uRn58vyzqpVEKUYKvVKnjswsICnj9/jqGhoXWX3D5+/IgXL17g5MmTqKysBM/zgmrabDY0NDQklVltiBGs1+tx9OhRQWODwSBqa2sxOzubcEw8HsfAwAAmJydx7949QWe/ioqKoNPpiFpFSsw+uLCwEBzHbTkuGo2ivr5+U7mrmZmZQX19vaDTkxzHobCwUFBdrUCM4CNHjggaZ7fb4fV6k6rt8Xjw/v17WXNoBWIEZ2dnCxr37t07UfXfvn0raw6tQIxgIcehPp8Pfr9fVH2/3w+fzydLDi1BjGAhM91AICCph5Dthc64tQIxgtPS0rYck8wtq2K3F5JDSxAjeHFxccsxu3btktRDyKGSkBxaghjBQu6DysnJEf0kWIPBgAMHDsiSQ0sQI1joce3p06dF1T9z5oysObQCMYLdbregcTabDXv27EmqdmZmJmw2m6w5tAIxgl0ul6BxOp0Od+7cEfyYX51Oh7t372Lnzp2y5tAKxAgOBAKCH2xy6NAhPHr0CLm5uZuOy8vLQ01NTcKbxtYyMTFB3D6YmIsNwN+zTYlu8FpLVlYWampq4HQ6E14uLC4uTqp/b2+vmNiqQpRgp9MJr9e75TdzNcXFxUmL3AiPx4Px8XHJdVINMT/RwN9LfG1tbZJPaCRLLBZDW1sbkbfMEiUYALxeL16+fJnSnp2dnfj27VtKe8oFcYIB4PXr1xgeHk5Jr6GhIfT09KSklxIQKTgej6O5uRmfPn1StM/o6CiePXtG5E/zCkQKBoDl5WU0NjZKfsxQIux2O5qamlK+v5cbombRa4nFYmhtbYXb7U5q8dxmrCzWGxwclCGh+hAteIWBgQG4XC5cvnwZJ06cELR2ay3Ly8sYHBxEV1eXKq/OUwoqBANAKBRCS0sLXr16hVOnTqG0tFTQ8prp6Wk4HA4MDAxIXjCgRagRvEIgEEB3dze6u7thMBiQm5u77hEOc3Nz8Pv98Hq9ir69VAtQJ3g1c3Nz1D1sPFmInUUzhMEEUw4TTDlMMOWIFkza+mBSEbrSJBGiBSvxChjGeqR+zqIFy3ERnbE1yd7wvhbRgi0WC/uZVhie52GxWCTVEC1Yr9en/GEo/xrV1dXQ6/WSakiaRZvNZlRVVSE9nU3G5SQ9PR1VVVUwm82Sa8nyajuPx4OOjo6k3tfH2JiCggJcvXoVeXl5stSTRfAKwWAQHo8n6ZdT/uusvJwyLy8Pu3fvlrW2rIIZ2oPtPCmHCaYcJphymGDKYYIphwmmHCaYcphgymGCKYcJphwmmHKYYMphgimHCaYcJphymGDKYYIphwmmHCaYcphgyvkfbSrtodI8KV4AAAAASUVORK5CYII='")
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
          img(:src="i.badgeIconUri || currentProject.logoUrl || 'data:image/png;base64,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'")
          .result-info
            strong {{ currentProject.name || "Platform name" }}
            h4 {{i.name}}
            em.small {{i.description}}
            //- p {{i}}

    div.mt-4(v-if="testResult.result")
      h3 JSON result
      .card
        pre(v-if="testResult.result.interactions") {{ JSON.stringify(testResult.result, null, 2) }}


</template>

<script lang="ts">
import { mapState, mapActions, mapGetters } from 'vuex'
import GoLiveButton from '../molecules/go-live-button.vue'
import PlatformToken from '../molecules/platform-token.vue'
import PlatformInstructions from '../molecules/platform-instructions.vue'

export default {
  computed: {
    ...mapState('projects', ['current', 'status', 'testStatus']),
    ...mapGetters('projects', [
      'currentProject',
      'currentPlatform',
      'testResult',
      'testError',
      'currentProjectCompleted'
    ]),
    formDisabled: function() {
      return this.editUrlDisabled || this.testStatus === 2 || this.submitted
    }
  },
  components: {
    GoLiveButton,
    PlatformToken,
    PlatformInstructions
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
        })
        .catch((error) => {
          alert(error)
          this.completed = true
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
      if (this.editUrlDisabled) {
        this.editUrlDisabled = false
      }
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

.tech-box {
  flex: 0 1 auto;
  order: 2;
  margin-left: 4rem;
}

.visual-result {
  flex: 1 0 50%;
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
    img {
      width: 80px;
      height: 80px;
      border: 1px solid $border-grey;
    }
  }
}

.result-wrapper {
  display: flex;
  flex-direction: row;
}
</style>
