<template lang="pug">
  div.test-application-page

    h1 Test your application
    p.
      To help you test the application you are developing to retrieve data from
      platforms for a user, we have created some test methods you can use to
      send test data to receive in your application.
    p.
      #[strong Tip!] #[em To test while developing on localhost, consider using a service like] #[a(href="https://ngrok.com/" target="_blank") ngrok] #[em to receive data from Open Platforms to your development machine.]
    current-application-tokens
    hr
    .card
      h2 Available test functions
      ol
        li
          strong Authenticate user with Open Platforms
        li
          strong Receive user's data from Open Platforms
    h2 Authenticate user with Open Platforms
    p.
      TODO: Write instructions for connecting with a user account through Auth0 to Open Platforms
      and approve the connection.
    app-auth-test(result="completed" buttonText="Send completed auth response")
    app-auth-test(result="cancelled" buttonText="Send cancelled auth response")

    hr

    h2 Receive user's data from Open Platforms
    p.
      TODO: Add a button to send updated user data to the application connection url.
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import CurrentApplicationTokens from '../components/organisms/current-application-tokens.vue'
import AppAuthTest from '@/components/organisms/app-auth-test.vue'

@Component({
  components: {
    CurrentApplicationTokens,
    AppAuthTest
  }
})
export default class TestApplicationPage extends Vue {}
</script>
