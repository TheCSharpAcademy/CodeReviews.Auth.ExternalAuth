import { QuestionReqDTO } from './QuestionReqDTO';

export interface QuizReqDTO {
  name: string;
  questions: QuestionReqDTO[];
}
