<template lang="pug">
  div#home
    #start-upper
      #start-title
        h1.color-import Open Platforms developer portal
        p Be part of our digital infrastructure enabling portability of data in the gig economy!
        //- p The open platforms API makes it possible to:


    #start-info
      strong Connect your platform
      p.
        Let your user share their experience and reputation
        from your platform by integrating with the
        #[strong.color-export Platform API].
      strong Register your application
      p.
        Let your user access their data from Open Platforms partners by
        connecting to the API. Read the
        #[a.color-import(href="https://gigdata-api.openplatforms.org/" title="Read the documentation for the application API")  Application API docs].
      button.btn.my-4.btn-huge.btn-project( v-if="$auth.isAuthenticated == false" @click="login") Get started!

 </template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions } from 'vuex'
import { getInstance } from '../auth/auth-wrapper'

export default {
  data() {
    return {
      user: {
        name: '',
        email: '',
        password: ''
      },
      submitted: false
    }
  },
  computed: {
    ...mapState('account', ['status']),
    ...mapState('projects', ['all'])
    // ...mapActions('account', ['register']),
  },
  methods: {
    // Log the user in
    login() {
      this.$auth.loginWithRedirect()
    }
  },
  async created() {
    const authService = getInstance()
    const fn = () => {
      // If the user is authenticated, continue with the route
      if (authService.isAuthenticated && this.all) {
        this.$router.push('/projects')
      } else if (authService.isAuthenticated) {
        this.$router.push('/create')
      }
    }
    authService.$watch('loading', (loading) => {
      if (loading === false) {
        return fn()
      }
    })
  }
}
</script>


<style lang="scss">
.nologin #main {
  background:#3b3b3b;
  @include small-screen-and-up {
    background: #3b3b3b url('../assets/img/open-platforms-logo-icon.svg') right center /
      55vmin auto no-repeat;
  }
  @include small-screen{
    background-size: 67wmin;
    background-position:top right;
  }
  display: flex;
  padding: 0;
  position: relative;

  #page {
    width: auto;
    max-width: none;
    z-index: 1;
  }
}
#home {
  color: #fff;
  @include flex(column, null, null);
  height: 100%;
  p {
    background-color: rgba(59, 59, 59, 0.8);
    background-color: #3b3b3b;
  }
  #start-upper {
    @include flex(row, space-between, flex-start);
    flex: 1 0 auto;
    #start-title {
      flex: 0 1 auto;

      h1 {
        font-size: 3rem;
      }
      p {
        font-size: 2rem;
      }
    }
    @include small-screen-and-up {
      align-items:center;
      #start-title {
        max-width: 56vw;

        h1 {
          font-size: 3.2vw;
        }
        p {
          font-size: 2.4vw;
          line-height: 1.2;
        }
      }
    }
    #sign-up {
      flex: 0 1 400px;
      margin: $space-big;

      form.card {
        background-color: $color-project;
        padding: 2rem 3rem;
        .btn {
          padding: 0;
        }
      }
    }
  }
  #start-info {
    flex: 1 0 auto;
    width: 40%;
    min-width:320px;

    @include small-screen{
    width: 100%;
    justify-content: flex-end;
    display: flex;
    flex-direction: column;
    }


    a {
      background-color: #3b3b3b;
      font-weight: bold;
      margin: 0 2px;
      white-space: nowrap;
    }
  }
}
</style>
