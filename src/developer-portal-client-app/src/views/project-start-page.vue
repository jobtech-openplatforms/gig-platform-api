<template lang="pug">
  div.home.project-start-page.center(v-if="currentProject")
    div
      h1 Project {{currentProject.name}} created!
      p Your project comes with a live and a test environment. The only thing shared between the environments is the project name. 
      .toggle-buttons.center
        button.btn-big.toggle-button.btn-outline.btn-test(@click="switchTestMode" v-bind:class="{ activestate: testMode}") Test mode
        button.btn-big.toggle-button.btn-outline.btn-live(@click="switchTestMode" v-bind:class="{ activestate: !testMode}") Live mode
      hr.my-4

      h2 Connections
      p You will need to have your platform connection live before you can use an application connection for retrieving real data, however you can absolutely start developing for it and test your work with the test data provided.

      .flex-wrapper

        .box
          h3.color-export Platform
          .frame-wrapper
            p Make sure your users can access their experience / reputation data on your platform. Suspendisse bibendum arcu a venenatis venenatis. Donec tincidunt justo consectetur rhoncus luctus. Nulla varius hendrerit sapien eget sagittis. Mauris quis maximus ipsum, sit amet porttitor ex. Etiam sit amet dui consequat, ultricies sem sed, hendrerit elit.
              router-link.color-export(to="/platform-documentation")  Read more
        .box
          h3.color-import Application
          .frame-wrapper
            p Get access to your user's data or let them access their data from other Open Platforms partners. Aliquam vehicula enim ac erat pellentesque mattis. Phasellus dignissim augue eget lectus cursus, quis accumsan elit lacinia. Nam blandit at urna sed posuere. Nam venenatis ipsum lacus, nec consequat nunc dictum ac. In non tempus velit. 
              router-link.color-import(to="/application-documentation")  Read more

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapGetters, mapMutations } from 'vuex'

@Component({
  computed: {
    ...mapState('projects', ['testMode']),
    ...mapGetters('projects', ['currentProject']),
    ...mapMutations('projects', ['switchMode'])

    
  },
  methods: {
    switchTestMode() {
      this.$store.commit('projects/switchMode', true)
    }
  },
})
export default class ProjectStartPage extends Vue {}
</script>

<style lang="scss">

</style>
