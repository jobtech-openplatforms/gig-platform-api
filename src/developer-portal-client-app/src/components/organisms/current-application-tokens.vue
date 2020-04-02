<template lang="pug">  
div(v-if="currentApplication")
  p To use the API you'll need these API keys:
  .token-keys
    .token-key
    .label-muted ApplicationId
    pre.color-import.shiny {{currentApplication.id}}
    .token-key
    .label-muted Secret key
    pre.color-import.shiny {{currentApplication.secretKey}}
</template>


<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapGetters } from 'vuex'

@Component({
  computed: {
    ...mapGetters('projects', ['currentApplication'])
  }
})
export default class CurrentApplicationTokens extends Vue {
  private currentApplication: any

}
</script>