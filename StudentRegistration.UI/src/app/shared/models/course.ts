import { Professor } from './professor';

export interface Course {
  id: string;
  name: string;
  credits: number;
  professor: Professor;
}