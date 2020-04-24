<template lang="pug">
  div#app(v-bind:class="{ nologin: !($auth.loading || $auth.isAuthenticated)}")
    TopNav
    //- Loading(:active.sync="loading" :can-cancel="false" :is-full-page="true" loader="dots")
    LoaderOverlay(v-if="loading")
    #main(v-bind:class="{noprojects : !hasProjects}")
      SideNav( v-if="!$auth.loading && $auth.isAuthenticated")
      #page
        div#alert-global( v-if="alert.message" :class="`alert ${alert.type}`" v-html="alert.message")
        transition(name="fade" mode="out-in")
          router-view
</template>

<script>
import TopNav from '@/components/molecules/top-nav.vue'
import SideNav from '@/components/molecules/side-nav.vue'
// import Loading from 'vue-loading-overlay'
import LoaderOverlay from '@/components/atoms/loader-overlay.vue'
import { mapState, mapActions, mapGetters } from 'vuex'

export default {
  name: 'app',
  components: {
    TopNav,
    SideNav,
    // Loading,
    LoaderOverlay
  },
  computed: {
    ...mapState({
      alert: (state) => state.alert
    }),
    ...mapState('account', ['status']),
    ...mapGetters('projects', ['loading', 'hasProjects'])
  },
  methods: {
    ...mapActions({
      clearAlert: 'alert/clear'
    })
  },
  watch: {
    $route(to, from) {
      // clear alert on location change
      this.clearAlert()
    }
  }
}
</script>

<style lang="scss">
@import './assets/scss/general.scss';

#alert-global{
  position:absolute;
  width:100vw;
  text-align:center;
  color:$color-warning;
  background:#fff;
  font-weight:400;
  top:0;
  left:0;
  right:0;
  padding:2rem;
}

#app{
  @include flex(column, null, null);
  min-height:100vh;
  background-color:#3b3b3b;
}

#main {
  @include flex(row, null, null);
  @include flex-child(1, null, null);
  // @include sidebar-width(padding-left);
  padding-left:$sidebar-width;
  overflow-x:hidden;
  max-width:$large-screen-width;
  background-color:$light-grey;
  position:relative;

  @include tiny-screen{
    padding-left:0;
  }

  &.noprojects{
      padding-left:0;
  }
}

#page{
  overflow:auto;
  @include flex-child(1, null, null);
  margin-left:auto;
  margin-right:auto;
  padding-top:8rem;
  padding-bottom:2rem;
  @include page-horizontal-spacing(margin-left margin-right);

  a{
    font-weight:bold;
    &:not([class^="btn"]){
      color:$color-project;
    }
  }

}

.fade-enter-active,
.fade-leave-active {
  transition-duration: 0.3s;
  transition-property: opacity;
  transition-timing-function: ease;
}

.fade-enter,
.fade-leave-active {
  opacity: 0
}
</style>
