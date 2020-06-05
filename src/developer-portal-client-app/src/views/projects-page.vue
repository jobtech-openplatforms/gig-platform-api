<template lang="pug">
  div.start
    .intro-text
      h1 Welcome to Open Platforms developer portal!
      p Through the Open Platforms API you can:
        ul
          li #[strong Connect your Platform] - Make sure your users can access their experience/reputation data on your platform.
          li #[strong Register your Application] - Get access to your user's data or let them access their data from other Open Platforms partners.
      .buttons.mt-4
        router-link.btn.btn-project.new-project( to="/create") New project
      p.small.mt-2.text-center 
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
  background-position: 75% 90%;
  background-size: 35% auto;
  background-repeat: no-repeat;
  position:absolute;
  top:0;
  left:0;
  right:0;
  bottom:0;

  .intro-text{
    position:relative;
    top:20%;
    padding-top:8rem;
    transform:translateY(-20%);
    @include page-horizontal-spacing('padding-left', 'padding-right');
  
  }

  @include medium-screen-and-up{
    background-image: url('../assets/img/open-platforms-logo-icon.svg');

    .intro-text{
      margin-left:$sidebar-width;
      padding-left:6vw;
      width: 55vw;
      min-width:460px;
      max-width:$tiny-screen-width;
    }
  }
}

</style>
