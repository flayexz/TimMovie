export interface UploadHooks{
    preview: string | null,
    setPreview: Function,
    file: File[],
    setFile: Function
}