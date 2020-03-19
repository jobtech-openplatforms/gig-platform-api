<template lang="pug">
  div.home.project-page.center(v-if="currentProject")

    h1 {{currentProject.name}}

    current-step(:step="nextStep")

    .flex-wrapper
      ProjectEdit

        hr
        .integration-status
          strong Platform&nbsp;
          span.yes {{currentPlatform && (currentPlatform.published === true) ? ' Active' : ''}}
          router-link.btn.btn-export.btn-outline.btn-small(to="/share-user-data") {{currentPlatform && currentPlatform.exportDataUri ? 'Edit...' : 'Add...'}}

        hr
        .integration-status
          strong Application&nbsp;
          span.yes {{currentProject.applications && currentProject.applications[0].authCallbackUrl ? 'Ready' : ''}}
          router-link.btn.btn-import.btn-outline.btn-small(to="/integrate-user-data") {{currentProject.applications && currentProject.applications[0].authCallbackUrl ? 'Edit...' : 'Add...'}}



</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
import ProjectEdit from '../components/organisms/project-edit.vue'
import CurrentStep from '../components/organisms/current-step.vue'

@Component({
  computed: {
    ...mapGetters('projects', ['currentProject', 'currentPlatform', 'nextStep'])
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

  .project {
    padding: 2rem;
    max-width: 560px;
    margin-left: auto;
    margin-right: auto;
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
