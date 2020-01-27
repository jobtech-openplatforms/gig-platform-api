<template lang="pug">
  .card
    slot
    form.inline( @submit.prevent="handleSubmit" v-if="!submitted")
      label.flex(for="get-user-data")
        .label-text Export data url
          //- span(v-if="current.loading")  (Loading...)
        input(type="text"
              :placeholder="currentPlatform ? currentPlatform.exportDataUri:'https://yourdomain.com/get-user-data'"
              :disabled="status === 2"
              id="get-user-data" v-model="exportDataUri")
        .feedback
      button.btn.btn-export(:disabled="status === 2") Save
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'

@Component({
  computed: {
    ...mapState('projects', ['current', 'status']),
    ...mapActions('projects', ['setPlatformUrl']),
    ...mapGetters('projects', ['currentProject', 'currentPlatform'])
  }
})
export default class OpenDataForm extends Vue {
  private submitted: boolean = false
  private ready: boolean = false
  private exportDataUri: string = ''
  private currentPlatform: any

  private mounted() {
    this.exportDataUri = this.currentPlatform
      ? this.currentPlatform.exportDataUri
      : ''
  }

  private handleSubmit(e) {
    if (!this.exportDataUri) {
      return
    }
    this.submitted = true
    this.$store
      .dispatch('projects/setPlatformUrl', this.exportDataUri)
      .then((result) => {
        this.$router.push('/test-open-api')
        this.submitted = false
      })
      .catch((error) => {
        this.submitted = false
        this.$store.dispatch('alert/error', error, { root: true })
      })
  }
}
</script>
