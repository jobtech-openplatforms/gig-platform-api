<template lang="pug">
  .instructions
    p #[router-link(to="/documentation") Documentation] #[strong &gt;] #[router-link.color-export(to="/platform-documentation") Platform documentation]
    div
      h1 Open Platforms platform integration documentation
      p.
        This documentation describes how to implement
        the integration with Open Platforms in your
        service.
    .frame.m-4
      h2 Table of contents
      ul.toc
        li #[a.color-export.page-nav(@click="goto('#implementation')") Implementation]
        li #[a.color-export.page-nav(@click="goto('#howitworks')") How it works]
          ul
            li #[a.color-export.page-nav(@click="goto('#your-implementation')") Your implementation]
        li #[a.color-export.page-nav( @click="goto('#prerequisites')") Prerequisites]
        li #[a.color-export.page-nav( @click="goto('#api')") API]
        li #[a.color-export.page-nav( @click="goto('#examples')") Examples]
    div
      h2#implementation Implementation
      p.
        Briefly described, this is how to create an integration
        with Open Platforms for your service.
      ul
        li.
          This implementation is primarily for platforms
          without an API or OAuth connections to users
          (gig workers). If your platform already has an
          API for users to connect with and retrieve their
          data on experience, earnings and reputation,
          please get in touch with us about setting up
          the integration.
        li.
          You will need to build a custom endpoint (URL)
          that receives a security token (to verify that
          the request is from Open Platforms) and an identifier
          for the user.
        li.
          If the identifier is the user's e-mail
          address, Open Platforms will have verified the user's
          ownership of that e-mail address before making
          requests for updates to your platform.
    hr.spacious

    div
      h2#howitworks How it works
      p.
        Briefly described, this is how the Open Platforms
        connection is established with your service.
      ol.card
        li.
          Create an endpoint (URL) that is publicly
          accessible for calls from Open Platforms to
          request data for a specified user.
          For testing we recommend using #[strong DEV mode].
          #[router-link.color-export(to="/platform-settings") Enter your endpoint url]
          for the platform.
        li.
          The endpoint verifies the security token
          from Open Platforms and returns an HTTP
          status in the 400-599 range (recommended: 401
          Unauthorized) if the token is incorrect.
        li.
          Based on the identifier (the user's e-mail address)
          your endpoint responds with data in the format
          #[router-link.color-export(to="/json-schema") specified by Open Platforms].
        li.
          You test the integration with the test tool
          provided #[router-link.color-export(to="/platform-test") here]
          by Open Platforms.
        li.
          You can test in both DEV mode and LIVE mode - this allows
          you to keep a separate development or staging server
          to test in DEV mode without having to release to
          a production server.
          #[br]
          #[em Please note that the PlatformToken is different for the DEV and LIVE mode setting]
        li.
          Once you have tested in both DEV and LIVE mode and
          the tests are successful, you can request for the
          platform to #[strong Go Live] to be integrated
          in apps chosen by the user.
        li.
          Open Platforms periodically retrieves updates
          from your platform for each connected user.
    hr.spacious


    div
      h2#prerequisites Prerequisites
      p.
        The following prerequisites are required for
        implementing the connection with Open Platforms
        and allowing your users to connect their data
        from your service.
      ul.card
        li.
          Your platform or service is accessible from the
          web and you are able to add a custom url to be
          called by Open Platforms with user data in the
          correct format.
        li.
          You are able to verify a header with a security
          token in the endpoint.
        li.
          Your platform has unique e-mail addresses for
          users so that there is no conflict between which
          account to retrieve user data from.

    hr.spacious

    div
      h2#api API
      p.
        Here is how you can expect a request body posted
        to your platform from Open Platforms to look like:
      pre
        ssh-pre(language="json" label="POST").
            {
              "PlatformToken": "0b0e2bda-e42b-431e-80b4-b2240e401990",
              "RequestId": "3b6ad503-eae9-4699-b5cc-a8b671c9e7e3",
              "UserEmail": "your-test-account@your-platform.tld"
            }
      p Of course the values will be different according to your platform and settings.
      p.
        Please see the #[router-link.color-export(to="/json-schema") JSON Schema]
        for how to format the response to requests from
        Open Platforms.

    h2#examples Examples
    h3 Javascript
    pre
      ssh-pre(language="js" label="Javascript").
        export function exportOpenPlatformsData(req: express.Request, res: express.Response) {
          const platform: string = req.body.PlatformToken;
          // replace with the PlatformToken for your Open Platforms integration
          if (platform === "OPEN-PLATFORMS-TOKEN") {
            const userEmail: string = req.body.UserEmail;
            // replace with your function that returns the user's interactions
            getUserInteractions(userEmail)
            .then((data) => {
              const dataString = JSON.stringify(data);
              res.send(dataString);
            })
            .catch((e) => {
              res.status(405).send("couldn't find interactions for user: " + userEmail); });
          } else {
            res.status(405).send("invalid platform token: " + platform);
          }
        }
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
import SshPre from 'simple-syntax-highlighter'
import 'simple-syntax-highlighter/dist/sshpre.css'

@Component({
  components: { SshPre },
})
export default class PlatformDocumentationPage extends Vue {
  goto(refName) {
    const el = document.querySelector(refName)
    if (el) el.scrollIntoView({ behavior: 'smooth', block: 'center' })
  }
}
</script>
