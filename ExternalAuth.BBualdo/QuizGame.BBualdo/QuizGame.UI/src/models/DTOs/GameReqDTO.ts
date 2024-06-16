export interface GameReqDTO {
  username: string | null;
  difficulty: 'Easy' | 'Medium' | 'Hard' | null;
  date: string | null;
  score: number | null;
  quizId: number | null;
}
