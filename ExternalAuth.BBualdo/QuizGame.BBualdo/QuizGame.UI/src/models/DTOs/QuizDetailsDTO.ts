import { Question } from '../Question';

export interface QuizDetailsDTO {
  quizId: number;
  name: string;
  questions: Question[];
}
