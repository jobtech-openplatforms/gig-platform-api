<template lang="pug">
div(v-if="currentApplication")
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
import { ApplicationState } from '@/store/projects.module'

@Component({
  computed: {
    ...mapGetters('projects', ['currentApplication'])
  }
})
export default class CurrentApplicationTokens extends Vue {
  private currentApplication?: ApplicationState

}
</script>
