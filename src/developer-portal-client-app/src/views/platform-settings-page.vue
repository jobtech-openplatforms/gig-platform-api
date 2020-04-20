<template lang="pug">
  div.home
    div(v-if="ready")
      h1 Platform API
      p To let your user's access their data on your platform you need to implement a simple export-API on your service.
      OpenDataForm
      div.mt-4(v-if="currentPlatform")
        .token-keys.mb-4
          PlatformToken
      PlatformInstructions
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
import FadeTransition from '../components/utils/fade-transition.vue'
import PlatformToken from '../components/molecules/platform-token.vue'
import OpenDataForm from '../components/organisms/open-data-form.vue'
import PlatformInstructions from '../components/molecules/platform-instructions.vue'

@Component({
  computed: {
    ...mapState('projects', ['current', 'status']),
    ...mapGetters('projects', ['currentProject', 'currentPlatform'])
  },
  async created() {
    await this.$store.dispatch('projects/initCurrentProject')
    if (!this.currentProject) {
      this.$router.push('/projects')
    } else if (this.currentPlatform && this.currentPlatform.exportDataUri) {
      this.$router.push('/platform-test')
    }
    this.ready = true
  },
  components: {
    FadeTransition,
    OpenDataForm,
    PlatformToken,
    PlatformInstructions
  }
})
export default class PlatformSettingsPage extends Vue {
  private submitted: boolean = false
  private ready: boolean = false
  private exportDataUri: string = ''

  private mounted() {
    this.exportDataUri = this.$store.state.projects.current.project.platforms
      ? this.$store.state.projects.current.project.platforms[0].exportDataUri
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
        this.submitted = false
      })
      .catch((error) => {
        this.submitted = false
        this.$store.dispatch('alert/error', error, { root: true })
      })
  }
}
</script>

<style lang="scss">
.token {
  color: $color-export;
  font-size: $font-big;
  font-family: $serif;
  text-align: center;
  font-weight: bold;
  margin-bottom: 6rem;
  margin-top: 3rem;
}
</style>
