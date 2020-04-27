<template lang="pug">
  div
    .card
      h1 Open Platforms request structure
      p.
        To implement Open Platforms in your application, 
        please read the documentation below.
      p Table of contents
      ul.toc
        li #[a(@click="goto('#requeststructure')") Request structure for calls to your application]
        li #[a(@click="goto('#howitworks')") How it works]
        li #[a(@click="goto('#prerequisites')") Prerequisites]
        li #[a(@click="goto('#api')") API]  
          ul
            li #[a(@click="goto('#api-available')") Get the list of available platforms]  
            li #[a(@click="goto('#api-platform')") Get info about a platform]  
        li #[a(@click="goto('#json-schema')") JSON Schema]
    .card
      h2#requeststructure Request structure for calls to your application
      p.
        This documentation describes how to implement
        the integration with Open Platforms in your
        application.
    .card
      h2#howitworks How it works
      p.
        Briefly described, this is how the Open Platforms
        connection is established with your service.
      ol
        li.
          Create three endpoints (URLs) that are publicly
          accessible for calls from Open Platforms to
          update data for a specified user.
          For testing we recommend using #[strong Test mode].
          #[router-link(to="/application-settings") Enter your endpoint url] 
          for the application.
        li.
          The endpoints should verifiy the application ID and Secret Key
          from Open Platforms and return an HTTP
          status in the 400-599 range (recommended: 401
          Unauthorized) if the token is incorrect.
        li.
          Based on the identifier (the user's e-mail address)
          the endpoint responds with data in the format
          specified by Open Platforms.
        li.
          You test the integration with the test tool
          provided #[router-link(to="/application-test") here]
          by Open Platforms.
        li.
          Once the test is successful, request for the
          application to #[strong Go Live] to be integrated
          in apps chosen by the user.
        li.
          Open Platforms periodically sends updates
          from your application for each connected user.
    .card
      h2#prerequisites Prerequisites
      p.
        The following prerequisites are required for
        implementing the connection with Open Platforms
        and allowing your users to connect their data
        from your service.
      ul
        li.
          Your application or service is accessible from the
          web and you are able to add custom urls to be
          called by Open Platforms with user data in the
          correct format.
        li.
          You are able to verify a header with authentication
          in the endpoint.
    .card
      h2#api API
      p.
        The base url for API calls for applications is 
        #[code https://openplatforms-gigdata-api-test.jobtechdev.se]
      p.
        Documentation with API endpoints is available 
        #[a(href="https://openplatforms-gigdata-api-test.jobtechdev.se/index.html" title="API documentation") here].
      h3#api-available Get the list of available platforms
      p API endpoint: #[code /api/Platform/available]
      p.
        Depending on your business model, you may choose to 
        allow your users to import gig data from all platforms
        available on Open Platforms or just the platforms that
        are relevant to you.
      pre
        code curl -X GET "https://openplatforms-gigdata-api-test.jobtechdev.se/api/Platform/available" -H "accept: application/json"
      h3#api-platform Get info about a platform
      p API endpoint: #[code /api/Platform/{platformId}]
      p.
        Get a single platform based on #[code platformId] -
        you can get the platform IDs from the #[code /available]
        endpoint.
      p CURL example:
      pre
        code curl -X GET "https://openplatforms-gigdata-api-test.jobtechdev.se/api/Platform/4365712f-68de-4888-bf81-b9e19dce1725" -H "accept: application/json"
    .card
      h2#json-schema JSON Schema
      p.
        #[strong TODO:] Describe the format for communication
        with applications
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'

@Component({})
export default class ApplicationDocumentationPage extends Vue {

  
    goto(refName) {
    	// var element = this.$refs[refName];
      const el = document.querySelector(refName)
      el && el.scrollIntoView({behavior: 'smooth', block: 'center'})
    }
}
</script>

<style lang="stylus" scoped>
pre
  border 1px solid grey
  background #ccc
.toc
  li
    cursor pointer
</style>