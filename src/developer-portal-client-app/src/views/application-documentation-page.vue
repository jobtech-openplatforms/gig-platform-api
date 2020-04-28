<template lang="pug">
  div
    .card
      h1 Open Platforms request structure
      p.
        To implement Open Platforms in your application, 
        please read the documentation below.
      p Table of contents
      ul.toc
        li #[a.page-nav(@click="goto('#requeststructure')") Request structure for calls to your application]
        li #[a.page-nav(@click="goto('#howitworks')") How it works]
          ul
            li #[a.page-nav(@click="goto('#your-implementation')")]
        li #[a.page-nav( @click="goto('#prerequisites')") Prerequisites]
        li #[a.page-nav( @click="goto('#api')") API]  
          ul
            li #[a.page-nav( @click="goto('#api-available')") Get the list of available platforms]  
            li #[a.page-nav( @click="goto('#api-platform')") Get a single platform]
            li #[a.page-nav( @click="goto('#api-callback')") Callback url]
            li #[a.page-nav( @click="goto('#api-update')") Data update url]
        li #[a.page-nav( @click="goto('#json-schema')") JSON Schema]
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
          To retrieve a 
          #[a.page-nav( @click="goto('#api-available')") list of available platforms] 
          that your users can connect to using Open Platforms, you
          can get a list from the #[code /available] API endpoint
          (#[a.page-nav( @click="goto('#api-available')") see details below]).
          #[br]
          To get details about a single platform, call the 
          #[a.page-nav( @click="goto('#api-platform')") endpoint]
          by platformId.

        li.
          Implement support for your users to connect to any platform
          (a generic solution) or select platforms that are relevant
          to your application.

        li.
          #[a.page-nav(@click="goto('#api-callback')") Create an endpoint URL] 
          for your application that receives
          the callback from Open Platforms with the parameters
          #[code result], #[code openplatformsuserid] and #[code requestid] 
          passed as GET variables.
          #[br]
          Example:
          #[br]
          #[pre https://example-application.app/open-platforms-callback?result=completed&openplatformsuserid=123zyx&requestid=your-reference]
        
        li.
          Fill out the callback url (without GET variables) in the 
          #[router-link(to="/application-settings") Application settings].

        li.
          Implement the data update url in your application to receives
          data updates for each user in your solution.

        li.
          Test the solution by clicking to connect a platform from your
          application interface. Login with your test user to Open
          Platforms and authenticate with the user's account in the
          platform.
          #[br]
          Include a #[code requestid] parameter that is unique to the request
          and will be returned as a result in the callback request 
          to your server.

        li.
          The test user is redirected to the approval screen for the connection
          between your application and the platform.

        li The end user is returned to your application in one of two ways:
          ul
            li.
              If the approval screen was opened in a popup window, the popup
              window is closed.
            li.
              If the approval screen was not opened in a popup window, the
              user is redirected to the callback url.

      h3#your-implementation Your implementation
      ul
        li.
          Create two endpoints (URLs) that are publicly
          accessible for calls from Open Platforms to
          update data for a specified user.
          For testing we recommend using #[strong Test mode].
          #[router-link(to="/application-settings") Enter your endpoint url] 
          for the application.
        li.
          The endpoints should verify the application ID and Secret Key
          from Open Platforms and return an HTTP
          status in the 400-599 range (recommended: 401
          Unauthorized) if the token is incorrect.
        li.
          Test the integration with the test tool
          provided #[router-link(to="/application-test") here]
          by Open Platforms.
        li.
          Open Platforms periodically sends updates
          to your application for each connected user.
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
      h3#api-platform Get a single platform
      p API endpoint: #[code /api/Platform/{platformId}]
      p.
        Get a single platform based on #[code platformId] -
        you can get the platform IDs from the #[code /available]
        endpoint.
      p CURL example:
      pre
        code curl -X GET "https://openplatforms-gigdata-api-test.jobtechdev.se/api/Platform/4365712f-68de-4888-bf81-b9e19dce1725" -H "accept: application/json"
      h3#api-callback Callback url
      p.
        The callback url is entered in the 
        #[router-link(to="/application-settings") Application settings].
      p.
        The callback url is used for responding to a request for a user
        connection from your application to a platform.
      p.
        To initiate a user connection from your application to a platform,
        generate a unique request ID for the connection and redirect the user
        to this Open Platforms url:
      pre
        code.
          https://openplatforms-user-test.jobtechdev.se/initiate-connection?requestid={requestid}&app={applicationid}&platform={platformid}&permissions=1
      p The #[code /initiate-connection] url requires these parameters:
      p
        code requestid
        span.
          the identifier for the request. Could be the userId or a generated
          request identifier if you need to refer to additional details
          in your system about the request.
      p
        code app
        span.
          your Open Platforms Application Id.
      p
        code platform
        span.
          the Platform Id for the platform to connect the user to.
      p
        code permissions
        span.
          1 for aggregated data, 2 for detailed data.
      p
        em.
          If your application opens a new window, the Open Platforms 
          user portal attempts to detect that, and close the window
          if that's the case. If so, a server side request is first
          sent to the callback url with the result of the connection.
      p
        em.
          If no popup window is detected, the Open Platforms user portal
          instead redirects to the callback url. 
      p In both cases above, the following parameters are included.
      pre
        code https://your-callback-url/?requestid={requestid}&openplatformsuserid={openplatformsuserid}&result={completed|failed}
      p.
        Parameters included in the callback url call:
      
      p
        code requestid
        span.
          the request identifier you sent as a reference in the initial call.
      p
        code openplatformsuserid
        span.
          the Open platforms user identifier. Save this for reference.
      p
        code result
        span.
          the result of the connection can be either #[code completed] or #[code failed].
        
      h3#api-update Data update url
          

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
      const el = document.querySelector(refName)
      el && el.scrollIntoView({behavior: 'smooth', block: 'center'})
    }
}
</script>

<style lang="stylus" scoped>
pre
  border 1px solid grey
  background #ccc
.page-nav
  cursor pointer
</style>