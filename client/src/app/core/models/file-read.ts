import {TimeStamp} from './time-stamp';

export interface FileRead extends TimeStamp{
  id: string;
  name: string;
  previewUrl?: string | null;
}
