<template lang="pug">
  div.home
    div(v-if="ready")
      ApitestForm(v-if="currentPlatform" :new-url="currentPlatform.exportDataUri")
</template>

<script lang="ts">
import ApitestForm from '../components/organisms/apitest-form.vue'
import { Component, Vue } from 'vue-property-decorator'
import { mapGetters } from 'vuex'

@Component({
  computed: {
    ...mapGetters('projects', ['currentPlatform'])
  },
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
