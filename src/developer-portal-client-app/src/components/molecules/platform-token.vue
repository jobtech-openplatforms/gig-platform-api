<template lang="pug">
  .token-key
    .label-muted.
      #[span(v-if="!testMode") LIVE]
      #[span(v-if="testMode") TEST mode]
      Platform token
    pre.color-export.shiny {{currentPlatform.platformToken}}
</template>

<script lang="ts">
import { mapState, mapActions, mapGetters } from 'vuex'
import Vue from 'vue'
export default Vue.extend({
  computed: {
    ...mapState('projects', ['testMode']),
    ...mapGetters('projects', ['currentPlatform'])
  },

})
</script>
