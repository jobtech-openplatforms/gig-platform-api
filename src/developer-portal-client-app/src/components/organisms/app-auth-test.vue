<template lang="pug">
  .app-test
    button.btn-import.btn-small.mt-2(@click="buttonClick" :disabled="loading") {{buttonText}}
    p.loading-text(v-if="loading") loading...
    .card.my-2(v-if="response && response.message")
      h4 {{response.message}}
      div(v-if="!response.success") Request to the server failed.
      div Tested URL:
        br
        code {{response.testedUrl}}
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
    error: '',
    loading: false
  }),
})
export default class AppAuthTest extends Vue {
  @Prop({ default: 'completed' }) readonly result!: string
  @Prop({ default: 'Submit' }) readonly buttonText!: string
  private currentProject?: ProjectState
  // tslint:disable-next-line:no-any
  response?: any
  error?: string
  loading: boolean

  private buttonClick() {
    this.response = {}
    this.error = ''
    this.loading = true
    const aTest = (this.result === 'completed') ?
      applicationTestService
        .testAuthentication(this.currentProject.id)
      :
      applicationTestService
        .testAuthenticationCancel(this.currentProject.id)

    aTest
        .then(
        (responseObj) => {
          const data = responseObj.data
          this.response = data
          this.loading = false
        },
        (error) => {
          // console.log(error)
          this.error = error && error.message ? error.message : error
          this.loading = false
        })
  }
}
</script>

