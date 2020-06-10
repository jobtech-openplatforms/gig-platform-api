<template lang="pug">
  .single-project

    div.home.project-page.center(v-if="currentProject")
      // h1 {{currentProject.name}}

      current-step(:step="nextStep")
      p Need help? #[router-link(to="/documentation") Read more in the full documentation]
      ProjectEdit

        hr
        .integration-status
          strong Platform
          span.yes.ml-1 {{currentPlatform && (currentPlatform.published === true) ? ' Active' : ''}}
          router-link.btn.btn-export.btn-outline.btn-small(to="/platform-settings") {{currentPlatform && currentPlatform.exportDataUri ? (currentPlatform.published ? 'Edit...' : 'Test...') : 'Add...'}}
        doc-platform

        hr
        .integration-status
          strong Application
          span.yes.ml-1 {{currentApplication && currentApplication.authCallbackUrl ? 'Ready' : ''}}
          router-link.btn.btn-import.btn-outline.btn-small(to="/application-settings" ) {{currentApplication && currentApplication.authCallbackUrl ? 'Edit...' : 'Add...'}}
        doc-application
      h2(style="margin-top:20px") Testing
      p.
        You can switch settings between live and a DEV mode.
        In DEV mode, there are no updates published through
        Open Platforms and is safe for development.
      p(v-if="!devMode") You are currently in #[strong LIVE mode]. Try switching modes for local development.
      p(v-if="devMode") You are currently in #[strong DEV mode]

      .toggle-buttons.text-center
        button.btn-big.toggle-button.btn-outline.btn-dev(@click="switchTestMode" v-bind:class="{ activestate: devMode}") DEV mode
        button.btn-big.toggle-button.btn-outline.btn-live(@click="switchTestMode" v-bind:class="{ activestate: !devMode}") Live mode
      hr.my-4

    div.home.project-page.center(v-else-if="currentProjects")
      .arrow
      p(class="space-above") Use the menu to select project and switch between Live and Dev mode.
      p For local development, **DEV mode**.


</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters, mapMutations } from 'vuex'
import ProjectEdit from '../components/organisms/project-edit.vue'
import CurrentStep from '../components/organisms/current-step.vue'
import DocPlatform from '@/components/molecules/doc-platform.vue'
import DocApplication from '@/components/molecules/doc-application.vue'

@Component({
  computed: {
    ...mapState('projects', ['devMode']),
    ...mapGetters('projects', ['currentProject', 'currentProjects', 'currentPlatform', 'currentApplication', 'nextStep'])
  },
  components: {
    ProjectEdit,
    CurrentStep,
    DocPlatform,
    DocApplication
  }
})
export default class ProjectPage extends Vue {
    switchTestMode() {
      this.$store.commit('projects/switchMode', true)
    }

}
</script>

<style lang="scss">
.single-project{
  max-width:$width-narrow;
  margin:auto;
}

</style>
