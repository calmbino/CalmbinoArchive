import type { PageLoad } from './$types';

export const load: PageLoad = ({ params, data }) => {
	console.log(`브라우저에서 slug가 ${params.slug}인 포스트 전처리 합니다`);
	return {
		serverData: data,
		clientData: {}
	};
};
