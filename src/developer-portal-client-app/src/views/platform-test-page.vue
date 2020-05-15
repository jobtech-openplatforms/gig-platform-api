<template lang="pug">
  div.home
    div(v-if="ready")
      ApitestForm
</template>

<script lang="ts">
import ApitestForm from '../components/organisms/apitest-form.vue'
import { Component, Vue } from 'vue-property-decorator'

@Component({
  data() {
    return {
      ready: false
    }
  },
  async created() {
    await this.$store.dispatch('projects/initCurrentProject')
    this.$store.commit('projects/resetTest')
    this.ready = true
  },
  components: {
    ApitestForm
  }
})
export default class PlatformTestPage extends Vue {}
</script>
