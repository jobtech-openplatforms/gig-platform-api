<template lang="pug">
  .single-project

    div.home.project-page.center(v-if="currentProject")
      // h1 {{currentProject.name}}

      current-step(:step="nextStep")

      ProjectEdit

        hr
        .integration-status
          strong Platform
          span.yes.ml-1 {{currentPlatform && (currentPlatform.published === true) ? ' Active' : ''}}
          router-link.btn.btn-export.btn-outline.btn-small(to="/platform-settings") {{currentPlatform && currentPlatform.exportDataUri ? (currentPlatform.published ? 'Edit...' : 'Test...') : 'Add...'}}

        hr
        .integration-status
          strong Application
          span.yes.ml-1 {{currentApplication && currentApplication.authCallbackUrl ? 'Ready' : ''}}
          router-link.btn.btn-import.btn-outline.btn-small(to="/application-settings" ) {{currentApplication && currentApplication.authCallbackUrl ? 'Edit...' : 'Add...'}}

    div.home.project-page.center(v-else-if="currentProjects")
      .arrow
      p(class="space-above") Use the menu to select project and switch between Live and Test mode.
      p For local development, **DEV mode**.


</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters, mapMutations } from 'vuex'
import ProjectEdit from '../components/organisms/project-edit.vue'
import CurrentStep from '../components/organisms/current-step.vue'

@Component({
  computed: {
    ...mapGetters('projects', ['currentProject', 'currentProjects', 'currentPlatform', 'currentApplication', 'nextStep'])
  },
  components: {
    ProjectEdit,
    CurrentStep
  }
})
export default class ProjectPage extends Vue {
  // private async createApplication() {
  //   await this.$store.dispatch('projects/createApplication')
  // }

}
</script>

<style lang="scss">
.single-project{
  max-width:$width-narrow;
  margin:auto;
}

</style>
