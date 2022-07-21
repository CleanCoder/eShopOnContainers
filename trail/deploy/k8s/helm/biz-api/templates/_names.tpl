{{- define "fqdn-image" -}}
{{- if .Values.image.registry -}}
{{- printf "%s/%s" .Values.image.registry.server .Values.image.repository -}}
{{- else -}}
{{- .Values.image.repository -}}
{{- end -}}
{{- end -}}