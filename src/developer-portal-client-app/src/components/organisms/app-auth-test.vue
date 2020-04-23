<template lang="pug">
  .app-test
    button(@click="buttonClick") {{buttonText}}
    div(v-if="response && response.message")
      p {{response.message}}
      p(v-if="!response.success") Request to the server failed.
      p Tested URL:
        br
        code {{response.testedUrl}}
      hr
    div {{error}}
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import { applicationTestService } from '../../_services'
import { mapGetters } from 'vuex'
import { ApplicationState, ProjectState } from '@/store/projects.module'


@Component({
  computed: {
    ...mapGetters('projects', ['currentProject'])
  },
  data: () => ({
    response: '',
    error: ''
  }),
})
export default class AppAuthTest extends Vue {
  @Prop({ default: 'completed' }) readonly result!: string
  @Prop({ default: 'Submit' }) readonly buttonText!: string
  private currentProject?: ProjectState
  response?: any
  error?: string

  private buttonClick() {
    this.response = {}
    this.error = ''
    if(this.result === 'completed')
      applicationTestService
        .testAuthentication(this.currentProject.id)
        .then(
        (responseObj) => {
          const data = responseObj.data
          this.response = data
        },
        (error) => {
          console.log(error)
          this.error = error && error.message ? error.message : error
        })
    else
      applicationTestService
        .testAuthenticationCancel(this.currentProject.id)
        .then(
        (responseObj) => {
          const data = responseObj.data
          if (data.success) {
            this.response = data.message
          }else {
            this.error = data.message
          }
        },
        (error) => {
          console.log(error)
          this.error = error && error.message ? error.message : error
        })
  }
}
</script>

<style>

</style>
