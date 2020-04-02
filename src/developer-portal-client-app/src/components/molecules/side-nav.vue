<template lang="pug">
  .side-nav( v-bind:class="{ testmode: testMode, livemode: !testMode}")
    #banner
      #banner-content(v-if="currentProject")
        em You are in 
        strong.test-text(v-if="testMode") TEST 
        strong.live-text(v-else) LIVE 
        em mode on project 
        strong {{currentProject.name}}
    #menu-head
      .my-projects( @click="redirectUser") Projects
      #mode-nav
        .mode.testmode(v-if="testMode")
          span.small.test-text TEST
          button.btn-clean.small.live-text( @click="switchTestMode") LIVE
        .mode.livemode(v-else)
          button.btn-clean.small.test-text( @click="switchTestMode") TEST
          span.small.live-text LIVE
    ul#projects-list(v-if="currentProjects")
      li.project(v-for="project in currentProjects" v-bind:class="{ active: current && currentProject && project.id === currentProject.id}")
        .project-bar(@click="setCurrentProject(project)")
          router-link.project-link(to="/project" v-if="project.logoUrl != null")
            //- img.project-logo(:src="project.logoUrl")
            div.project-logo(v-if="project.logoUrl != null" :style="{'background-image': 'url(' + project.logoUrl + ')'}")

          .project-name {{project.name}} 
            .small.test-text(v-if="testMode") [TEST] 
          .connections(v-bind:class="{hasplconn : project.platforms && project.platforms.length >= 1 && project.platforms[0] && (project.platforms[0].published || (testMode && project.platforms[0].exportDataUri))}")
          .connections(v-bind:class="{hasappconn : project.applications && project.applications.length >= 1 && project.applications[0] && project.applications[0].authCallbackUrl}")
        .details(v-if="current && current.project && project.id === current.project.id")
          hr
          router-link.color-export( to="/share-user-data" active-class="active" v-bind:class="{ 'active': $route.path == '/test-open-api' }") Platform API
          router-link.color-import( to="/integrate-user-data" active-class="active") Application API
          //- router-link.color-project( to="/project" active-class="active") project info
    div.new-project
      router-link( to="/create" v-if="!testMode") + New project
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters, mapMutations } from 'vuex'
import { State, Mutation, namespace } from 'vuex-class'

@Component({
  computed: {
    ...mapState('projects', ['current', 'all', 'status', 'testMode']),
    ...mapGetters('projects', ['currentProject', 'currentProjects', 'currentApplication', 'currentPlatform']),
    ...mapMutations('projects', ['switchMode'])
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
    },
    switchTestMode() {
      this.$store.commit('projects/switchMode')
    }
  },
  async created() {
    await this.getAll()
    this.ready = true
  }
})
export default class SideNav extends Vue {
}
</script>

<style lang="scss">

.test-text{
  color:$color-test;
}

.live-text{
  color:$color-live;
}

#banner{
  position:absolute;
  top:0;
  left:0;
  background:rgba(0,0,0,0.45);
  padding-left:calc(#{$sidebar-width} + 6rem);
  width:100vw;

  #banner-content{
    max-width:$media-small;
    margin-left:auto;
    margin-right:auto;
  }
}

#menu-head{
  display:flex;
  flex-direction:row;
  justify-content:space-between;
  align-items:center;
  margin: 6rem 1rem 1rem;

  .mode > *:last-child{
    margin-left:1rem;
  }

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
  background: #3b3b3b;
  left: 0;
  float: left;
  width: $sidebar-width;
  color: $white;
  height: calc(100vh - 60px);
  position: fixed;
  border-right:1.5rem solid $bg-color;
  &.livemode {
    border-color:$color-live;
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
      height: 65px;
      overflow: hidden;
      &:not(:last-child) {
        margin-bottom: 1rem;
      }

      &.active {
        background: rgba(255, 255, 255, 0.075);
        height: 150px;
      }
      .project-bar {
        @include flex(row, flex-start, center);
        cursor: pointer;
        padding:0 1rem;
        height:$project-menu-logo-height;

        .connections {
          border-radius: 50%;
          width: 1rem;
          height: 1rem;
          margin-left: 0.5rem;
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

        .project-link{
          width: $project-menu-logo-width;
          height: $project-menu-logo-height;
          margin-right:1.5rem;
        }

        .project-logo {
          width: 100%;
          height:100%;
          margin-right:2rem;
          margin-left:-1rem;
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
