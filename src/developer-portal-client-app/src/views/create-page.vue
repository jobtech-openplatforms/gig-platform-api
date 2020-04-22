<template lang="pug">
  div.home.narrow(v-if="ready")
    h1 Create a project
    p.
      First, start by giving your project a name. 
      This is most probably the name of your platform/service. You can always change this later.
    CreateProjectForm


</template>

<script lang="ts">
import CreateProjectForm from '../components/organisms/create-project-form.vue'
import { Component, Vue } from 'vue-property-decorator'

@Component({
  computed: {},
  data() {
    return {
      ready: false
    }
  },
  methods: {
    // ...mapActions('projects', ['currentProject'])
  },
  async created() {
    this.$store.commit('projects/changeCurrentProject', null)
    this.ready = true
  },
  components: {
    CreateProjectForm
  }
})
export default class CreatePage extends Vue {}
</script>

