import {
  Column,
  Entity,
  Index,
  JoinColumn,
  ManyToOne,
  OneToMany,
} from "typeorm";
import { CommentReports } from "./CommentReports";
import { AspNetUsers } from "./AspNetUsers";
import { Films } from "./Films";

@Index("IX_Comments_AuthorId", ["authorId"], {})
@Index("IX_Comments_FilmId", ["filmId"], {})
@Index("PK_Comments", ["id"], { unique: true })
@Entity("Comments", { schema: "public" })
export class Comments {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("uuid", { name: "FilmId" })
  filmId: string;

  @Column("uuid", { name: "AuthorId" })
  authorId: string;

  @Column("text", { name: "Content" })
  content: string;

  @Column("timestamp with time zone", { name: "Date" })
  date: Date;

  @OneToMany(() => CommentReports, (commentReports) => commentReports.comment)
  commentReports: CommentReports[];

  @ManyToOne(() => AspNetUsers, (aspNetUsers) => aspNetUsers.comments, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "AuthorId", referencedColumnName: "id" }])
  author: AspNetUsers;

  @ManyToOne(() => Films, (films) => films.comments, { onDelete: "CASCADE" })
  @JoinColumn([{ name: "FilmId", referencedColumnName: "id" }])
  film: Films;
}
