import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Comment } from 'src/app/models/comment';
import { ImageSnippet } from 'src/app/models/image-snippet';
import { Location } from 'src/app/models/location';
import { LostDogService } from 'src/app/services/lost-dog-service';

@Component({
  selector: 'app-lost-dog-comment',
  templateUrl: './lost-dog-comment.component.html',
  styleUrls: ['./lost-dog-comment.component.css']
})
export class LostDogCommentComponent implements OnInit {
  addCommentForm = new FormGroup({
    text: new FormControl('', [Validators.required]),
    locationCity: new FormControl('', [Validators.required]),
    locationDistrict: new FormControl('', [Validators.required]),
  });
  @Output() onSubmitComment = new EventEmitter<any>();
  @Output() onCancelComment = new EventEmitter<any>();
  @Input() lostDogID?: number;
  selectedFile!: ImageSnippet;
  url!: any;
  isPictureChosen: boolean = false;

  constructor(
    private lostDogService: LostDogService
  ) { }

  ngOnInit(): void {
  }

  onSubmit() {
    this.lostDogService.addCommentToLostDog(this.constructCommentRequest(), this.lostDogID!).subscribe(response => {
      this.onSubmitComment.emit();
    });
  }

  private constructCommentRequest(): FormData {
    const location = new Location(
      this.addCommentForm.get('locationCity')?.value,
      this.addCommentForm.get('locationDistrict')?.value
    );
    const comment = new Comment(
      this.addCommentForm.get('text')?.value,
      location
    );
    let data = new FormData();
    // works only with our Backend
    // data.append('comment', JSON.stringify(comment));
    data.append("comment", new Blob([JSON.stringify(comment)], { type: "application/json", }), "");
    if (this.isPictureChosen) {
      data.append('picture', this.selectedFile.file);
    }
    return data;
  }

  onCancel() {
    this.onCancelComment.emit();
  }

  processFile(event: any) {
    const file: File = event.target.files[0];
    const reader = new FileReader();

    reader.addEventListener('load', (even: any) => {
      this.selectedFile = new ImageSnippet(even.target.result, file);
    });
    
    reader.readAsDataURL(file);
    reader.onload = (event: any) => {
      this.url = reader.result;
    }

    this.isPictureChosen = true;
  }
}
