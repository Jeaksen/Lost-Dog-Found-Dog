export class Picture {
    fileName: string;
    fileType: string;
    data: string;

    constructor(fileName: string, fileType: string, data: string) {
        this.fileName = fileName;
        this.fileType = fileType;
        this.data = data;
    }
}