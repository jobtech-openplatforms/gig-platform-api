<template lang="pug">
  div.start
    //- .tile(v-if="!all.projects")
    .tile
      .intro-text
        h2 Welcome to Open Platforms developer portal!
        p Through the Open Platforms API you can:
          ul
            li #[strong Connect your Platform] - Make sure your users can access their experience/reputation data on your platform.
            li #[strong Register your Application] - Get access to your user's data or let them access their data from other Open Platforms partners.
        .buttons.mt-4
          router-link.btn.btn-project.new-project( to="/create") New project
        p.small.mt-2 
          em This version of the service is desktop only. 
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapMutations } from 'vuex'

@Component({
  computed: {
    ...mapState('projects', ['current', 'all', 'status']),
    ...mapState('account', ['status', 'user'])
  },
  data() {
    return {
      ready: false
    }
  },
  methods: {
    ...mapActions('projects', ['getAll']),
    loadProject(p) {
      this.$store.dispatch('projects/setCurrentProject', p)
    }
  },
  async created() {
    if (this.current && this.current.project) {
      this.$router.push('/project')
    }
    await this.getAll()
    this.ready = true
  },
  components: {}
})
export default class ProjectsPage extends Vue {}
</script>

<style lang="scss">
.start {
  background-image: url('../assets/img/open-platforms-symbol.svg');
  background-position: 100% 80%;
  background-size: 70% auto;
  min-height: 100%;
  background-repeat: no-repeat;
  .intro-text{
    width:400px;
  }
}

</style>
