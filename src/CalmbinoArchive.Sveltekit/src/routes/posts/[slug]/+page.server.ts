import type { PageServerLoad } from './$types';
import { posts } from '../posts';

export const load: PageServerLoad = async ({ params }) => {
	const post = posts.find((post) => post.slug == params.slug);

	console.log(`서버에서 slug가 ${params.slug}인 포스트 찾습니다.`);

	return { post };
};
