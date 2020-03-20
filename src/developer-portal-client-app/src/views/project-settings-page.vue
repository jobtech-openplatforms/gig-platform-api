<template lang="pug">
  div.settings
    div(v-if="ready")
      h1 So, just a little about your service:
      p We want to rajdidaj Evam me sutam. Tamaham janami passami. Ye hi keci, bhikkhave, samana va brahmana va uddhamaghatanika sannivada uddhamaghatanam sannim attanam pannapenti, sabbe te imeheva solasahi vatthuhi, etesam va annatarena. Hoti kho so, bhikkhave, samayo, yam kadaci karahaci.
      ProjectSettings
      //- OpenDataForm
      //-   h2 Open data api settings
      //-   p Settings to open data from your platform to allow your users data mobility.
      .buttons
        .btn.btn-huge.btn-import.center.my-3 Go live!
      .card(style="max-width:500px;")
        h2 (det här ska tas bort)...
        pre(style="overflow:hidden;") {{ currentProject }}



</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
import ProjectSettings from '../components/organisms/project-settings.vue'
import OpenDataForm from '../components/organisms/open-data-form.vue'

@Component({
  computed: {
    ...mapState('projects', ['current', 'status']),
    ...mapGetters('projects', ['currentProject']),
    ...mapActions('projects', ['initCurrentProject', 'resetPlatformTest'])
  },
  data() {
    return {
      ready: false,
      imageData: null
    }
  },
  async created() {
    await this.$store.dispatch('projects/initCurrentProject')
    await this.$store.dispatch('projects/resetPlatformTest')
    if (!this.current || !this.current.project) {
      this.$router.push('/projects')
    }
    this.ready = true
  },
  components: {
    ProjectSettings,
    OpenDataForm
  }
})
export default class ProjectSettingsPage extends Vue {}
</script>



