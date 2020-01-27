<template lang="pug">
  .token-key
    .label-muted Platform token
    pre.color-export.shiny {{currentPlatform.platformToken}}
</template>

<script lang="ts">
import { mapState, mapActions, mapGetters } from 'vuex'
import Vue from 'vue'
export default Vue.extend({
  computed: {
    ...mapGetters('projects', ['currentPlatform'])
  },

})
</script>
