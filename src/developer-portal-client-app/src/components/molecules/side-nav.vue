<template lang="pug">
  .side-nav(v-if="currentProjects.length > 0" v-bind:class="{ testmode: devMode, livemode: !devMode, open: mobileMenuOpen}")
    #banner
      button#mobile-menu(@click="toggleMobileMenu()")


      #banner-content(v-if="currentProject").
        #[em You are in] #[strong.dev-text(v-if="devMode") DEV] #[strong.live-text(v-else) LIVE] #[em mode on project] {{currentProject.name}}
    #menu-head
      #banner-switch.toggle-buttons
        button.btn-tiny.toggle-button.btn-outline-reverse.btn-dev(@click="switchTestMode" v-bind:class="{ activestate: devMode}") DEV
        button.btn-tiny.toggle-button.btn-outline-reverse.btn-live(@click="switchTestMode" v-bind:class="{ activestate: !devMode}") Live
      .my-projects( @click="redirectUser") Projects
    ul#projects-list(v-if="currentProjects")
      li.project(v-for="project in currentProjects" v-bind:class="{ active: current && currentProject && project.id === currentProject.id}")
        .project-bar(@click="setCurrentProject(project); toggleMobileMenu()")
          router-link.project-link(to="/project")
            span.project-logo(v-if="project.logoUrl != null" :style="{'background-image': 'url(' + project.logoUrl + ')'}")
            span.project-logo(v-else)

          .project-name {{project.name}}
            .small.dev-text(v-if="devMode") [DEV]
          .connection-wrapper
            .connections(v-bind:class="{hasplconn : project.platforms && project.platforms.length >= 1 && project.platforms[0] && (project.platforms[0].published || (devMode && project.platforms[0].exportDataUri))}")
            .connections(v-bind:class="{hasappconn : project.applications && project.applications.length >= 1 && project.applications[0] && project.applications[0].authCallbackUrl}")
        .details(v-if="current && current.project && project.id === current.project.id")
          hr
          router-link.color-project( to="/project" active-class="active") Project info
          router-link.color-export( to="/platform-settings" active-class="active" v-bind:class="{ 'active': $route.path == '/platform-test' }")
            span Platform API
            .connections(v-bind:class="{hasplconn : project.platforms && project.platforms.length >= 1 && project.platforms[0] && (project.platforms[0].published || (devMode && project.platforms[0].exportDataUri))}")
          router-link.color-import( to="/application-settings" active-class="active")
            span Application API
            .connections(v-bind:class="{hasappconn : project.applications && project.applications.length >= 1 && project.applications[0] && project.applications[0].authCallbackUrl}")

    .new-project
      div(@click="toggleMobileMenu()")
        router-link( to="/create") + New project
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters, mapMutations } from 'vuex'
import { State, Mutation, namespace } from 'vuex-class'

@Component({
  computed: {
    ...mapState('projects', ['current', 'all', 'status', 'devMode']),
    ...mapGetters('projects', ['currentProject', 'currentProjects', 'currentApplication', 'currentPlatform']),
    ...mapMutations('projects', ['switchMode'])
  },
  data() {
    return {
      ready: false,
      mobileMenuOpen: false
    }
  },

  methods: {
    ...mapActions('projects', ['getAll', 'setCurrentProject'])
  },
  async created() {
    await this.getAll()
    this.ready = true
  }
})
export default class SideNav extends Vue {
  private mobileMenuOpen: boolean

    redirectUser() {
      this.$store.dispatch('projects/unsetCurrentProject').then((result) => {
        this.$router.push('/projects')
      })
    }

    switchTestMode() {
      this.$store.commit('projects/switchMode')
    }

    toggleMobileMenu(){
      this.mobileMenuOpen = !this.mobileMenuOpen
    }
}
</script>

<style lang="scss">

.dev-text{
  color:$color-test;
}

.live-text{
  color:$color-live;
}

#banner{
  position:fixed;
  top:$topbar-height;
  left:0;
  background:rgba(0,0,0,0.45);
  width:100vw;
  min-height:3rem;
  z-index:2;
  @include flex(row, flex-start, center);

  #mobile-menu{
    display:none;
    @include tiny-screen{
      display:block;
      background:transparent url('../../assets/img/menu.svg') left center / contain no-repeat;
      width:20px;
      height:20px;
      margin:1.5rem;
      border:0;
    }
  }

    #banner-switch{
        @include sidebar-width(width);
        padding:0.5rem 1rem 0.75rem;
        white-space:nowrap;
      }


  #banner-content{
    margin-right:auto;
    margin-left:auto;
    @include tiny-screen{
      margin:0;
      padding:0;
      line-height:normal;
    }
  }
}

#menu-head{
  @include flex(row, flex-start, center);
  padding: 6rem 1rem 1rem;
  position:relative;

  button{
    color:$dimmed-grey;
    transition: color 0.3s ease;
  }
  .testmode {
    span{
      color:$color-test;
    }
    button:hover{
      color:$color-live;
    }
  }
  .livemode {
    span{
      color:$color-live;
    }
    button:hover{
      color:$color-test;
    }
  }
}

.side-nav {
  @include sidebar-width(width);
  background: #3b3b3b;
  left: 0;
  float: left;
  color: $white;
  height: calc(100vh - #{$topbar-height});
  position: fixed;
  transition: all 0.2s ease;
  border-right:1px solid;
  z-index:2;


  &:not(.open){
    @include tiny-screen{
      left:-$sidebar-width-small;
    }
  }

  &.livemode {
    border-right:1.5rem solid $color-live;
  }

  @include flex(column, null, null);
  .my-projects {
    font-size: 2rem;
    padding:1rem;
    font-family:$serif;
    font-weight:bold;
    @include flex(row, null, center);
    flex: 0 1 auto;
  }
  a {
    color: $white;
    font-weight: 400;
  }

  .new-project {
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

    a{
      white-space:nowrap;
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
      height: 62px;
      overflow: hidden;
      &:not(:last-child) {
        margin-bottom: 1rem;
      }

      &.active {
        background: rgba(255, 255, 255, 0.075);
        @include small-screen-and-up{
          height: 165px;
          .connection-wrapper{
            display:none;
          }
        }
        .connections{
          margin:0.5rem 0;
        }
      }


      .connections {
          border-radius: 50%;
          width: 1rem;
          height: 1rem;
          margin: 0.5rem 0 0.5rem 0.5rem;
          justify-self: end;
          flex: 0 0 1rem;
          background-color:#3b3b3b;

          &.hasplconn {
            background-color: $color-export;
          }

          &.hasappconn {
            background-color: $color-import;
          }
        }

      .project-bar {
        @include flex(row, flex-start, center);
        cursor: pointer;
        // padding-right:1rem;
        height:$project-menu-logo-height;

        .project-link{
          @include flex-child(0, 0, auto);
          width: $project-menu-logo-width;
          height: $project-menu-logo-height;
          margin-right:1.5rem;
          @include tiny-screen{
             width: calc(#{$project-menu-logo-width} * 2 / 3);
             height: calc(#{$project-menu-logo-height} * 2 / 3);
          }
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
          @include flex(row, space-between, center);
          padding: 0.25rem 0 0.25rem 0;
          white-space: nowrap;

          &.active {
            font-weight: bold;
          }

          @include small-screen-and-up{
            margin-left: 6rem;
          }
        }
        hr {
          color: #3b3b3b;
          border-width: 0 0 1px;
          border-color: #3b3b3b;
          height: 1px;
          margin: 1rem 0;
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
