import { AnswerReqDTO } from './AnswerReqDTO';

export interface QuestionReqDTO {
  name: string;
  answers: AnswerReqDTO[];
}
