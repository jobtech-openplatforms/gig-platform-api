<template lang="pug">
  .single-project

    div.home.project-page.center(v-if="currentProject")
      // h1 {{currentProject.name}}

      current-step(:step="nextStep")

      .flex-wrapper
        ProjectEdit

          hr
          .integration-status
            strong Platform&nbsp;
            span.yes {{currentPlatform && (currentPlatform.published === true) ? ' Active' : ''}}
            router-link.btn.btn-export.btn-outline.btn-small(to="/share-user-data") {{currentPlatform && currentPlatform.exportDataUri ? (currentPlatform.published ? 'Edit...' : 'Test...') : 'Add...'}}

          hr
          .integration-status
            strong Application&nbsp;
            span.yes {{currentApplication && currentApplication.authCallbackUrl ? 'Ready' : ''}}
            router-link.btn.btn-import.btn-outline.btn-small(to="/integrate-user-data") {{currentApplication && currentApplication.authCallbackUrl ? 'Edit...' : 'Add...'}}

    div.home.project-page.center(v-else-if="currentProjects")
      .arrow
        img(class="arrow-image" src="../assets/img/arrow-drawn.png")
      p(class="space-above") Use the menu to select project and switch between Live and Test mode.
      p For local development, **Test mode**.


</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
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
export default class ProjectPage extends Vue {}
</script>

<style lang="scss">
.project-page {
  height: 100%;
  .flex-wrapper {
    flex-wrap: wrap;
    height: 100%;
    align-items: center;
  }

  .arrow {
    padding: 17px 0 5px 0; 
  }
  .arrow-image { 
    display: block;
    float: left;
    left: 300px;
    position: absolute;
  }
  .space-above {
    margin-top:100px;
  }

  .project {
    padding: 2rem;
    max-width: 560px;
    // margin-left: auto;
    // margin-right: auto;
    flex-grow: 1;

    .edit-logo:after {
      color: #fff;
      bottom: 0;
      top: unset;
    }

    .project-logo {
      width: 78px;
      height: 78px;
      flex: 0 0 78px;
      display: block;
      cursor:pointer;
      background-color:#fff;
      margin-right:2rem;
    }

    .integration-status {
      @include flex(row, null, center);
      margin: 2rem 0;

      .connection-type {
        flex: 1;
      }

      .btn {
        margin-left: auto;
      }
    }
  }

  .box:not(.project) {
    margin: 0 0 4rem;
    width: 100%;
    @include flex(column, null, null);
    @include small-screen() {
      width: 45%;
    }
    @include medium-screen-and-up() {
      width: 35%;
    }

    .frame {
      padding: $space-med;
      @include flex(column, null, null);

      p {
        flex: 1;
        margin: 0;
      }
    }
  }

  .delimiter {
    margin: 4rem 0;
  }

  h4 {
    margin-left: 2rem;
  }
}
</style>
