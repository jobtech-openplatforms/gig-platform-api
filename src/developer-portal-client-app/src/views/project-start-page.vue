<template lang="pug">
  div.home.project-start-page.center(v-if="currentProject")
    div
      h1 Project {{currentProject.name}}
      p Your project comes with a live and a test environment. The only thing shared between the environments is the project name.
      .toggle-buttons.text-center
        button.btn-big.toggle-button.btn-outline.btn-test(@click="switchTestMode" v-bind:class="{ activestate: testMode}") Test mode
        button.btn-big.toggle-button.btn-outline.btn-live(@click="switchTestMode" v-bind:class="{ activestate: !testMode}") Live mode
      hr.my-4

      h2 Connections
      p You will need to have your platform connection live before you can use an application connection for retrieving real data, however you can absolutely start developing for it and test your work with the test data provided.

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
