<script lang="ts">
  import type {PageProps} from "./$types";

  let { data }:PageProps = $props();
</script>

<h2>{data.serverData?.post?.title ?? "데이터가 존재하지 않습니다."}</h2>