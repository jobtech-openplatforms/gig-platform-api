<template lang="pug">
  .side-nav
    a.btn.my-projects( @click="redirectUser") Projects
    //- router-link.btn.my-projects( to="/projects") Projects

    ul#projects-list(v-if="all && all.projects")
      li.project(v-for="project in all.projects" v-bind:class="{ active: current && current.project && project.id === current.project.id}")
        .project-bar(@click="setCurrentProject(project)")
          router-link(to="/project")
            //- img.project-logo(:src="project.logoUrl")
            div.project-logo(:style="{'background-image': 'url(' + project.logoUrl + ')'}")

          .project-name {{project.name}}
          .hasplconn.connections(v-if="project.platforms && project.platforms[0].published")
          .hasappconn.connections(v-if="project.applications")
        .details(v-if="current && current.project && project.id === current.project.id")
          hr
          router-link.color-export( to="/share-user-data" active-class="active" v-bind:class="{ 'active': $route.path == '/test-open-api' }") Platform API
          router-link.color-import( to="/integrate-user-data" active-class="active") Application API
          //- router-link.color-project( to="/project" active-class="active") project info
    router-link.new-project( to="/create") + New project
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'

@Component({
  computed: {
    ...mapState('projects', ['current', 'all', 'status']),
    ...mapGetters('projects', ['currentProject'])
  },
  data() {
    return {
      ready: false
    }
  },
  methods: {
    ...mapActions('projects', ['getAll', 'setCurrentProject']),
    redirectUser() {
      this.$store.dispatch('projects/unsetCurrentProject').then((result) => {
        this.$router.push('/projects')
      })
    }
  },
  async created() {
    await this.getAll()
    this.ready = true
  }
})
export default class SideNav extends Vue {}
</script>

<style lang="scss">
.side-nav {
  background: #3b3b3b;
  left: 0;
  float: left;
  width: $sidebar-width;
  color: $white;
  height: calc(100vh - 60px);
  position: fixed;
  @include flex(column, null, null);
  .my-projects {
    margin: 1rem 1rem 1rem 6rem;
    font-size: 2rem;
    @include flex(row, null, center);
    flex: 0 1 auto;
  }
  a {
    color: $white;
    font-weight: 400;

    &.new-project {
      float: right;
      padding: 1rem;
      display: block;
      flex: 1 0 auto;
      z-index: 2;
      text-align: right;
      margin-top: -1rem;
      padding-top: 2rem;
      background: -moz-linear-gradient(
        top,
        rgba(59, 59, 59, 0) 0%,
        rgba(59, 59, 59, 1) 25%,
        rgba(59, 59, 59, 1) 100%
      );
      background: -webkit-linear-gradient(
        top,
        rgba(59, 59, 59, 0) 0%,
        rgba(59, 59, 59, 1) 25%,
        rgba(59, 59, 59, 1) 100%
      );
      background: linear-gradient(
        to bottom,
        rgba(59, 59, 59, 0) 0%,
        rgba(59, 59, 59, 1) 25%,
        rgba(59, 59, 59, 1) 100%
      );
      filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#003b3b3b', endColorstr='#3b3b3b',GradientType=0 );
    }
  }
  #projects-list {
    margin: 0;
    padding: 0;
    flex: 0 1 auto;
    overflow-x: hidden;
    overflow-y: auto;
    scrollbar-width: none;
    &::-webkit-scrollbar {
      display: none;
    }
    .project {
      background: rgba(0, 0, 0, 0.34);
      display: block;
      padding: 1rem;
      transition: all 0.3s ease-in-out;
      height: 70px;
      overflow: hidden;
      &:not(:last-child) {
        margin-bottom: 1rem;
      }

      &.active {
        background: rgba(255, 255, 255, 0.075);
        height: 165px;
      }
      .project-bar {
        @include flex(row, flex-start, center);
        cursor: pointer;

        .connections {
          border-radius: 50%;
          width: 1rem;
          height: 1rem;
          margin-left: 0.5rem;
          justify-self: end;
          flex: 0 0 1rem;
          &.hasplconn {
            background-color: $color-export;
          }

          &.hasappconn {
            background-color: $color-import;
          }
        }

        .project-logo {
          width: 5rem;
          height: 5rem;
          margin-right:2rem;
        }

        .project-name {
          color: $white;
          font-weight: bold;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
          min-width: 0;
          flex: 1 1 auto;
        }
      }
      .details {
        padding-bottom: 1rem;
        a {
          display: block;
          margin-left: 7rem;
          padding: 0.5rem 0;
          white-space: nowrap;

          &.active {
            font-weight: bold;
          }
        }
        hr {
          color: #3b3b3b;
          border-width: 0 0 1px;
          border-color: #3b3b3b;
          height: 1px;
          margin-bottom: 1rem;
        }
      }
      .color-export {
        color: $color-export;
      }
      .color-import {
        color: $color-import;
      }
      .color-project {
        color: $color-project;
      }
    }
  }
}
</style>
