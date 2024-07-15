import { Game } from '../Game';

export interface PaginatedGames {
  total: number;
  games: Game[];
}
