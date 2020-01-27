<template lang="pug">
  JsonSchema
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
import JsonSchema from '../components/atoms/json-schema.vue'

@Component({
  components: {
    JsonSchema
  }
})
export default class JsonSchemaPage extends Vue {}
</script>
