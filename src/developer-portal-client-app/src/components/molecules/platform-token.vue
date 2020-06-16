<template lang="pug">
  .token-key
    .label-muted.
      #[span(v-if="!devMode") LIVE mode]
      #[span(v-if="devMode") DEV mode]
      Platform token
    pre.color-export.shiny {{currentPlatform.platformToken}}
</template>

<script lang="ts">
import { mapState, mapActions, mapGetters } from 'vuex'
import Vue from 'vue'
export default Vue.extend({
  computed: {
    ...mapState('projects', ['devMode']),
    ...mapGetters('projects', ['currentPlatform'])
  },

})
</script>
