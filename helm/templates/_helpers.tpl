{{/*
Expand the name of the chart.
*/}}
{{- define "bymseread.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "bymseread.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "bymseread.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "bymseread.labels" -}}
helm.sh/chart: {{ include "bymseread.chart" . }}
{{ include "bymseread.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "bymseread.selectorLabels" -}}
app.kubernetes.io/name: {{ include "bymseread.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create component specific names
*/}}
{{- define "bymseread.service.name" -}}
{{- printf "%s-service" (include "bymseread.fullname" .) }}
{{- end }}

{{- define "bymseread.webclientapp.name" -}}
{{- printf "%s-web-client-app" (include "bymseread.fullname" .) }}
{{- end }}

{{- define "bymseread.dbmigrator.name" -}}
{{- printf "%s-dbmigrator" (include "bymseread.fullname" .) }}
{{- end }}

{{/*
Component selector labels
*/}}
{{- define "bymseread.service.selectorLabels" -}}
app.kubernetes.io/name: {{ include "bymseread.name" . }}-service
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{- define "bymseread.webclientapp.selectorLabels" -}}
app.kubernetes.io/name: {{ include "bymseread.name" . }}-web-client-app
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{- define "bymseread.dbmigrator.selectorLabels" -}}
app.kubernetes.io/name: {{ include "bymseread.name" . }}-dbmigrator
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}