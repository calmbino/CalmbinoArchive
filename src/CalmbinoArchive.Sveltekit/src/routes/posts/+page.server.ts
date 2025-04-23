import type { PageServerLoad } from './$types';
import { posts } from './posts';

export const load: PageServerLoad = async () => {
	console.log('포스트 목록을 조회합니다.');

	return { posts };
};
