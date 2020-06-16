<template lang="pug">
  div.home.project-start-page.center(v-if="currentProject")
    div
      h1 Project {{currentProject.name}}
      p.
        You can switch settings between live and a DEV mode.
        In DEV mode, there are no updates published through
        Open Platforms and is safe for development.
      p.
        Open Platforms provides testing functionality to send
        data to your platform or application so you can check
        that your server responds as expected.
      p.
        We recommend that you use DEV mode for local development
        with the Open Platforms API.
      p  #[router-link(to="/documentation") Read more in the full documentation]
      p(v-if="!devMode") You are currently in #[strong LIVE mode]. Try switching modes for local development.
      p(v-if="devMode") You are currently in #[strong DEV mode]

      .toggle-buttons.text-center
        button.btn-big.toggle-button.btn-outline.btn-dev(@click="switchTestMode" v-bind:class="{ activestate: devMode}") Dev mode
        button.btn-big.toggle-button.btn-outline.btn-live(@click="switchTestMode" v-bind:class="{ activestate: !devMode}") Live mode
      hr.my-4

      h2 Connections
      p.
        There are two types of integrations with Open Platforms.
        A Platform provides user data on experience, reputation
        and income for connected users.
        An Application allows users to retrieve their data from
        platforms to use with the application.

      docs-intro

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapGetters, mapMutations } from 'vuex'
import DocsIntro from '@/components/organisms/docs-intro.vue'

@Component({
  components: {
    DocsIntro
  },
  computed: {
    ...mapState('projects', ['devMode']),
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
